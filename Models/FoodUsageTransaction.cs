using System.ComponentModel.DataAnnotations;

namespace SocietyHopeOrg.Models
{
    public class FoodUsageTransaction
    {
        [Key]
        public int TransactionId { get; set; } // Unique identifier for each transaction

        [Required]
        [Display(Name = "Meal Time")]
        public string MealTime { get; set; } // Options: Breakfast, Lunch, Dinner

        [Required]
        [Display(Name = "Usage Date")]
        public DateTime UsageDate { get; set; } = DateTime.Now; // Date of the usage

        // Quantities used for each food type
        [Display(Name = "Canned Foods Quantity Used")]
        public int CannedFoodQuantityUsed { get; set; }

        [Display(Name = "Beans Quantity Used")]
        public int BeansQuantityUsed { get; set; }

        [Display(Name = "Pasta or Rice Quantity Used")]
        public int PastaOrRiceQuantityUsed { get; set; }

        [Display(Name = "Sugar, Flour, Salt, Condiments Quantity Used")]
        public int SugarFlourSaltCondimentsQuantityUsed { get; set; }

        [Display(Name = "Baby Products Quantity Used")]
        public int BabyProductsQuantityUsed { get; set; }

        // Available quantities after the transaction
        [Display(Name = "Available Canned Foods Quantity")]
        public int AvailableCannedFoodQuantity { get; set; }

        [Display(Name = "Available Beans Quantity")]
        public int AvailableBeansQuantity { get; set; }

        [Display(Name = "Available Pasta or Rice Quantity")]
        public int AvailablePastaOrRiceQuantity { get; set; }

        [Display(Name = "Available Sugar, Flour, Salt, Condiments Quantity")]
        public int AvailableSugarFlourSaltCondimentsQuantity { get; set; }

        [Display(Name = "Available Baby Products Quantity")]
        public int AvailableBabyProductsQuantity { get; set; }
    }
}
