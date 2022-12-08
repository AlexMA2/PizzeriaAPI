using PizzeriaAPI.Utils;

namespace PizzeriaAPI.Services.PizzaService
{
    public interface IPizzaService
    {
        Task<ResponseData<List<PizzaDto>>> GetAll();
        Task<ResponseData<PizzaDto>> GetById(int id);
        Task<ResponseData<string>> Create(Pizza pizza);
        Task<ResponseData<string>> Update(int id, PizzaRequest pizza);
        Task<ResponseData<string>> Delete(int id);

    }
}
