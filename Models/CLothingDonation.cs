using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SocietyHopeOrg.Models
{
    public class ClothingDonation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Clothing Type")]
        public ClothsTypes ClothType { get; set; }

        [Required]
        [DisplayName("Size")]
        public Sizes Size { get; set; } // Changed to enum

        [DisplayName("Tracking Code")]
        public string TrackingCode { get; set; } = "Unavailable"; // Default to "Unavailable"



    }
    public enum ClothsTypes
    {
        Vest,
        T_Shirt,
        Hoodie,
        Long_Pants,
        Short_Pants,
        Button_up,
        Long_Skirt,
        Short_Skirt
    }

    public enum Sizes
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }
}
