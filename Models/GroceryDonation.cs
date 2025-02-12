using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SocietyHopeOrg.Models
{
    public class GroceryDonation
    {
        [Key]
        public int DonationId { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Tracking Code")]
        public string TrackingCode { get; set; } = "Unavailable";

        // Quantity fields for each FoodType
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int CannedFoodQuantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int BeansQuantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int PastaOrRiceQuantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int SugarFlourSaltCondimentsQuantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int BabyProductsQuantity { get; set; } = 0;
    }

}
