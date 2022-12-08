using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaAPI.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        public ICollection<PizzaIngredient> Pizzas { get; set; } = null!;

    }
    public class IngredientRequest
    {
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public List<IngredientPizzaResponse> Pizzas { get; set; } = null!;
    }
}