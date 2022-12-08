using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PizzeriaAPI.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }      
        public bool IsPizzaOfTheWeek { get; set; }
        public int Amount { get; set; }
        public virtual ICollection<PizzaIngredient> Ingredients { get; set; } = null!;      
    }

    public class PizzaRequest
    {
        public string Name { get; set; } = null!;             
        public int Amount { get; set; }
        public ICollection<PizzaIngredientRequest> PizzaIngredientsRequest{ get; set; } = null!;
    }

    public class PizzaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public bool IsPizzaOfTheWeek { get; set; }
        public int Amount { get; set; }
        public virtual List<PizzaIngredientResponse> Ingredients { get; set; } = null!;

    }
}
