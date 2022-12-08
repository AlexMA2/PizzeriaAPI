using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaAPI.Models
{
    public class PizzaIngredient
    {
        [Key]
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;
        
        [Key]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
        
        [Column(TypeName = "decimal(5,2)")]       
        public decimal Cost { get; set; }
    }

    public class PizzaIngredientRequest
    {       
        public int IngredientId { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Cost { get; set; }
    }


    public class PizzaIngredientResponse
    {        
        public int IngredientId { get; set; }       
            
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
      
        public int Weight { get; set; }
    }
    
    public class IngredientPizzaResponse
    {
        public int PizzaId { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public bool IsPizzaOfTheWeek { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }

    }
}

