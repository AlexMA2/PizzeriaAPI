using Microsoft.EntityFrameworkCore;
using PizzeriaAPI.Data;
using PizzeriaAPI.Models;
using PizzeriaAPI.Utils;

namespace PizzeriaAPI.Services.IngredientService
{
    public class IngredientService : IIngredientService
    {
        private readonly PizzeriaContext _context;
        public IngredientService(PizzeriaContext pizzeriaContext)
        {
            _context = pizzeriaContext;
        }
        public async Task<ResponseData<string>> Create(Ingredient ingredient)
        {
            _context.Ingredient.Add(ingredient);
            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Ingredient created" };
            }
            return new ResponseData<string> { Success = true, Reason = ResponseCode.InternalServerError };
        }

        public async Task<ResponseData<string>> Delete(int id)
        {
            var ingredient = await _context.Ingredient.FindAsync(id);
            if (ingredient == null)
            {
                return new ResponseData<string> { Success = false, Reason = ResponseCode.NotFound };
            }
            _context.Ingredient.Remove(ingredient);
            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Ingredient deleted" };
            }
            return new ResponseData<string> { Success = true, Reason = ResponseCode.InternalServerError };
        }

        public async Task<ResponseData<List<IngredientDto>>?> GetAll()
        {
            var ingredients = await _context.Ingredient
                .Include(i => i.Pizzas)
                .ThenInclude(p => p.Pizza)
                .ToListAsync();
            if (ingredients.Count == 0)
            {
                return new ResponseData<List<IngredientDto>> { Success = false, Reason = ResponseCode.NotFound };
            }
            List<IngredientDto> ingredientsList = new();
            foreach (var ingredient in ingredients)
            {
                var ingredientDto = new IngredientDto
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Price = ingredient.Price,
                    Pizzas = ingredient.Pizzas.Select(x => new IngredientPizzaResponse
                    {
                        PizzaId = x.PizzaId,
                        Name = x.Pizza.Name,
                        Price = x.Pizza.Price,
                        IsPizzaOfTheWeek = x.Pizza.IsPizzaOfTheWeek,
                        Amount = x.Pizza.Amount,
                        Weight = (int)(x.Cost * 100 / ingredient.Price)
                    }).ToList()
                };
                ingredientsList.Add(ingredientDto);
            }
            return new ResponseData<List<IngredientDto>> { Success = true, Value = ingredientsList };
        }

        public async Task<ResponseData<IngredientDto>?> GetById(int id)
        {
            var ingredient = await _context.Ingredient
                .Where((i) => i.Id == id)
                .Include(i => i.Pizzas)
                .ThenInclude(p => p.Pizza)
                .FirstOrDefaultAsync();

            if (ingredient is null) {
                return new ResponseData<IngredientDto> { Success = false, Reason = ResponseCode.NotFound };
            }
            
            IngredientDto ingredientDto = new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Price = ingredient.Price,
                Pizzas = ingredient.Pizzas.Select(x => new IngredientPizzaResponse
                {
                    PizzaId = x.PizzaId,
                    Name = x.Pizza.Name,
                    Price = x.Pizza.Price,
                    IsPizzaOfTheWeek = x.Pizza.IsPizzaOfTheWeek,
                    Amount = x.Pizza.Amount,
                    Weight = (int)(x.Cost * 100 / ingredient.Price)
                }).ToList()
            };
            return new ResponseData<IngredientDto> { Success = true, Value = ingredientDto };
        }

        public async Task<ResponseData<string>> Update(int id, IngredientRequest ingredient)
        {
            var ingredientFound = await _context.Ingredient.Where((i) => i.Id == id).FirstOrDefaultAsync();

            if (ingredientFound is null)
            {
                return new ResponseData<string> { Success = false, Reason = ResponseCode.NotFound };
            }

            ingredientFound.Name = ingredient.Name;
            ingredientFound.Price = ingredient.Price;

            var entries = await _context.SaveChangesAsync();
            if (entries > 0)
            {
                return new ResponseData<string> { Success = true, Value = "Pizza updated" };
            }
            return new ResponseData<string> { Success = false, Reason = ResponseCode.InternalServerError };
        }
    }
}
