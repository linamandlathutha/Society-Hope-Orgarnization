using System.ComponentModel.DataAnnotations;

namespace SocietyHopeOrg.Models
{
    public class FoodUsageViewModel
    {
        [Required]
        [Display(Name = "Meal Time")]
        public string MealTime { get; set; } // Options could be "Breakfast", "Lunch", "Dinner"

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        [Display(Name = "Canned Foods Quantity to Use")]
        public int? CannedFoodQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        [Display(Name = "Beans Quantity to Use")]
        public int? BeansQuantity { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        [Display(Name = "Pasta or Rice Quantity to Use")]
        public int? PastaOrRiceQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        [Display(Name = "Sugar, Flour, Salt, Condiments Quantity to Use")]
        public int? SugarFlourSaltCondimentsQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        [Display(Name = "Baby Products Quantity to Use")]
        public int? BabyProductsQuantity { get; set; }
    }
}
