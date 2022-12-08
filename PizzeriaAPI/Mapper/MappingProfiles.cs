using AutoMapper;

namespace PizzeriaAPI.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Pizza, PizzaResponse>();
            //    cfg.CreateMap<PizzaRequest, Pizza>();
            //    cfg.CreateMap<PizzaIngredient, PizzaIngredientResponse>();
            //    cfg.CreateMap<PizzaIngredientRequest, PizzaIngredient>();
            //    cfg.CreateMap<Ingredient, IngredientResponse>();
            //}).CreateMapper();
            CreateMap<Pizza, PizzaDto>();
            CreateMap<IngredientRequest, Ingredient>();
        }
    }
}
