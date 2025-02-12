using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SocietyHopeOrg.Models
{
    public class ToyDonation
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
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of toys")]
        [DisplayName("Number of Toys")]
        public int NumberOfToys { get; set; }

        [DisplayName("Tracking Code")]
        public string TrackingCode { get; set; } = "Unavailable"; // Default to "Unavailable"
    }
}
