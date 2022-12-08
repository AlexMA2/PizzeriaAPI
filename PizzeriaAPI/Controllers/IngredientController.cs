using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PizzeriaAPI.Services.IngredientService;
using PizzeriaAPI.Services.PizzaService;
using PizzeriaAPI.Utils;
using System.Linq;

namespace PizzeriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;
        private readonly IMapper _mapper;

        public IngredientController(IIngredientService service, IMapper mapper)
        {

            _ingredientService = service;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<IngredientDto>>> Get()
        {
            ResponseData<List<IngredientDto>> ingredients = await _ingredientService.GetAll();

            if (ingredients.Success)
            {
                return Ok(ingredients.Value);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        // GET: IngredientController/Details/5
        public async Task<ActionResult<IngredientDto>> Get(int id)
        {
            ResponseData<IngredientDto> ingredients = await _ingredientService.GetById(id);

            if (ingredients.Success)
            {
                return Ok(ingredients.Value);
            }
            return NotFound();
        }

        [HttpPost]
        // GET: IngredientController/Create
        public async Task<ActionResult<string>> Post([FromBody] IngredientRequest ingredientRequest)
        {
            ResponseData<string> ingredient = await _ingredientService.Create(_mapper.Map<Ingredient>(ingredientRequest));

            if (ingredient.Success)
            {
                return Ok(ingredient.Value);
            }
            if (ingredient.Reason == ResponseCode.InternalServerError)
            {
                return StatusCode(500);
            }
            return NotFound();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Update(int id, [FromBody] IngredientRequest ingredientRequest)
        {
            ResponseData<string> ingredient = await _ingredientService.Update(id, ingredientRequest);

            if (ingredient.Success)
            {
                return Ok(ingredient.Value);
            }
            if (ingredient.Reason == ResponseCode.InternalServerError)
            {
                return StatusCode(500);
            }
            return NotFound();
        }

        // GET: IngredientController/Delete/5
        [HttpDelete]
        public async Task<ActionResult<string>> Delete(int id)
        {
            ResponseData<string> ingredient = await _ingredientService.Delete(id);
            if (ingredient.Success)
            {
                return Ok(ingredient.Value);
            }
            if (ingredient.Reason == ResponseCode.InternalServerError) {
                return StatusCode(500);
            }
            return NotFound();
        }

    }
}
