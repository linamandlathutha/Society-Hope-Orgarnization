using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocietyHopeOrg.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        [Required]
        public string Surname { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("PhoneNumber")]
        public string PhoneNumber {  get; set; }

        [Required(ErrorMessage = "Please choose profile image")]
        public string ProfilePicture { get; set; }

        [DisplayName("Gender")]
        public GenderType Gender { get; set; }
        public string? UserId { get; internal set; }

        public enum GenderType
        {
            Male,
            Female,
            Other
        }
    }
}
