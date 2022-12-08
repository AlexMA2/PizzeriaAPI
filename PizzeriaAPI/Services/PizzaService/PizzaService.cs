using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PizzeriaAPI.Data;
using PizzeriaAPI.Models;
using PizzeriaAPI.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PizzeriaAPI.Services.PizzaService
{
    public class PizzaService : IPizzaService
    {
        private readonly PizzeriaContext _context;
        public PizzaService(PizzeriaContext pizzeriaContext) {
            _context = pizzeriaContext;
        }
        public async Task<ResponseData<string>> Create(Pizza pizza)
        {
            Console.WriteLine(pizza.Name);
            _context.Pizza.Add(pizza);           
          
            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Pizza created" };
            }
            return new ResponseData<string> { Success = true, Reason = ResponseCode.InternalServerError };            
        }

        public async Task<ResponseData<string>> Delete(int id)
        {
            var pizza = await _context.Pizza.FindAsync(id);
            if (pizza == null)
            {
                return new ResponseData<string> { Success = false, Reason = ResponseCode.NotFound};
            }
            _context.Pizza.Remove(pizza);
            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Pizza deleted" };
            }
            return new ResponseData<string> { Success = true, Reason = ResponseCode.InternalServerError };
        }

        public async Task<ResponseData<List<PizzaDto>>> GetAll()
        {
            var pizzas = await _context.Pizza.Include(p => p.Ingredients).ThenInclude(i => i.Ingredient).ToListAsync();

            if (pizzas.Count == 0)
            {
                return new ResponseData<List<PizzaDto>> { Success = false, Reason = ResponseCode.NotFound };
            }
            
            List<PizzaDto> pizzasList = new();

            foreach (var pizza in pizzas)
            {
                var pizzaDto = new PizzaDto
                {
                    Id = pizza.Id,
                    Name = pizza.Name,
                    Price = pizza.Price,
                    IsPizzaOfTheWeek = pizza.IsPizzaOfTheWeek,
                    Amount = pizza.Amount,
                    Ingredients = pizza.Ingredients.Select(x => new PizzaIngredientResponse
                    {
                        IngredientId = x.IngredientId,
                        Name = x.Ingredient.Name,
                        Price = x.Ingredient.Price,
                        Weight = (int)(x.Cost * 100 / x.Ingredient.Price)
                    }).ToList()
                };
                pizzasList.Add(pizzaDto);
            }

            return new ResponseData<List<PizzaDto>> { Success = true, Value = pizzasList };
        }
        
        public async Task<ResponseData<PizzaDto>> GetById(int id)
        {
            var pizza = await _context.Pizza
                .Where((p) => p.Id == id).Include(p => p.Ingredients).ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync();
            
            if (pizza is null) {
                return new ResponseData<PizzaDto> { Success = false, Reason = ResponseCode.NotFound };
            }
         
            PizzaDto pizzaDto = new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price,
                IsPizzaOfTheWeek = pizza.IsPizzaOfTheWeek,
                Amount = pizza.Amount,
                Ingredients = pizza.Ingredients.Select(x => new PizzaIngredientResponse
                {
                    IngredientId = x.IngredientId,
                    Name = x.Ingredient.Name,
                    Price = x.Ingredient.Price,
                    Weight = (int)(x.Cost * 100 / x.Ingredient.Price)
                }).ToList()
            };

            return new ResponseData<PizzaDto> { Success = true, Value = pizzaDto };
        }
        
        public async Task<ResponseData<string>> Update(int id, PizzaRequest pizza)
        {
            var pizzaFound = await _context.Pizza.Where((p) => p.Id == id).Include(p => p.Ingredients).FirstOrDefaultAsync();

            if (pizzaFound is null)
            {
                return new ResponseData<string> { Success = false, Reason = ResponseCode.NotFound };
            }

            pizzaFound.Name = pizza.Name;          
            pizzaFound.Amount = pizza.Amount;          
           
            List<PizzaIngredient> newIngredients = pizza.PizzaIngredientsRequest.Select(pi => new PizzaIngredient
            {
                PizzaId = pizzaFound.Id,
                IngredientId = pi.IngredientId,
                Cost = pi.Cost
            }).ToList();
         
            
            pizzaFound.Ingredients = newIngredients;
            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Pizza updated" };
            }
            return new ResponseData<string> { Success = false, Reason = ResponseCode.InternalServerError };
        }
    }
}
