using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PizzeriaAPI.Services.PizzaService;
using PizzeriaAPI.Utils;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
           
        private readonly IPizzaService _pizzaService;
        private readonly IMapper _mapper;

        public PizzaController(IPizzaService service, IMapper mapper)
        {
            
            _pizzaService = service;
            this._mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<PizzaDto>>> Get()
        {
            ResponseData<List<PizzaDto>> pizzas = await _pizzaService.GetAll();                      

            if (pizzas.Success)
            {
                //var pizzasMapped = _mapper.Map<List<PizzaDto>>(pizzas.Value);
                return Ok(pizzas.Value);
            }
            return NotFound();
        }

        // GET api/<PizzaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaDto>> Get(int id)
        {
            ResponseData<PizzaDto> pizza = await _pizzaService.GetById(id);
            if (pizza.Success)
            {              
                return Ok(pizza.Value);
            }
            
            return NotFound();
        }

        // POST api/<PizzaController>
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] PizzaRequest pizzaRequest)
        {
            Console.WriteLine(pizzaRequest.Name);
            var pizzas = pizzaRequest.PizzaIngredientsRequest.ToArray();
            decimal total = 0m;
            decimal profit = 1.50m;
            for (var i = 0; i < pizzas.Length; i++) {
                total += pizzas[i].Cost;
            }
            total = profit * total;
            var pizza = new Pizza
            {
                Name = pizzaRequest.Name,
                Price = total,
                Amount = pizzaRequest.Amount,
                IsPizzaOfTheWeek = false,
                Ingredients = pizzaRequest.PizzaIngredientsRequest.Select(x => new PizzaIngredient { PizzaId = -1, IngredientId = x.IngredientId, Cost = x.Cost }).ToList()
            };

            ResponseData<string> response = await _pizzaService.Create(pizza);
            if (response.Success)
            {
                return Ok(response.Value);
            }
            return NotFound(response.Reason.ToString());          
        }

        // PUT api/<PizzaController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromBody] PizzaRequest pizza)
        {
            var response = await _pizzaService.Update(id, pizza);
            if (response.Success)
            {
                return Ok(response.Value);
            }
            return NotFound(response.Reason.ToString());
        }
        
        // DELETE api/<PizzaController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)            
        {
            var response = await _pizzaService.Delete(id);
            if (response.Success)
            {
                return Ok(response.Value);
            }
            return NotFound(response.Reason.ToString());
        }
    }
}
