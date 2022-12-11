using Microsoft.Extensions.Caching.Memory;
using PizzeriaAPI.Models;
using PizzeriaAPI.Utils;

namespace PizzeriaAPI.Services.IngredientService
{
    public class IngredientCache : IIngredientService
    {
        private readonly IMemoryCache _cacheManager;
        private readonly IngredientService _ingredientService;
        public IngredientCache(IMemoryCache cacheManager, IngredientService ingredientService)
        {
            _cacheManager = cacheManager;
            _ingredientService = ingredientService;
        }

        public Task<ResponseData<string>> Create(Ingredient ingredient)
        {
            return _ingredientService.Create(ingredient);
        }
        
        public Task<ResponseData<string>> Delete(int id)
        {
            return _ingredientService.Delete(id);
        }

        public Task<ResponseData<List<IngredientDto>>?> GetAll()
        {
            string key = "IngredientAll";
            return _cacheManager.GetOrCreateAsync(key, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return await _ingredientService.GetAll();
            });
        }

        public Task<ResponseData<IngredientDto>?> GetById(int id)
        {
            String key = $"Ingredient-{id}";
            Console.WriteLine(key);
            return _cacheManager.GetOrCreateAsync(key, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return await _ingredientService.GetById(id);
            });

        }

        public Task<ResponseData<string>> Update(int id, IngredientRequest ingredient)
        {
           return _ingredientService.Update(id, ingredient);
        }
    }
}
