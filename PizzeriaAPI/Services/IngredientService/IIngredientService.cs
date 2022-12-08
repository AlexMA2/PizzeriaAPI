using PizzeriaAPI.Utils;

namespace PizzeriaAPI.Services.IngredientService
{
    public interface IIngredientService
    {
        Task<ResponseData<List<IngredientDto>>> GetAll();
        Task<ResponseData<IngredientDto>> GetById(int id);
        Task<ResponseData<string>> Create(Ingredient ingredient);
        Task<ResponseData<string>> Update(int id, IngredientRequest ingredient);
        Task<ResponseData<string>> Delete(int id);
    }
}
