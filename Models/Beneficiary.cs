using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace SocietyHopeOrg.Models
{
    public enum BeneReason
    {
        Homeless,
        DrugUse,
        MentalHealth,
        Abuse
    }
    public class Beneficiary
    {
        [Key]
        public int BeneCount { get; set; }

        [DisplayName("Beneficiary Name")]
        [Required]
        [StringLength(50)]
        public string BeneName { get; set; }

        [DisplayName("Beneficiary Surname")]
        [Required]
        [StringLength(50)]
        public string BeneSurname { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "The Beneficiary ID must be exactly 13 characters long.")]
        public string BeneficiaryID { get; set; } // RSA ID number

        [Required]
        public bool IsMale { get; set; }

        [DisplayName("Beneficiary Reason")]
        [Required]
        public BeneReason BeneReasons { get; set; }
       

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } // Optional birthdate for age verification

        [DisplayName("Contact Number")]
        [Phone]
        public string ContactNumber { get; set; } // Optional contact number

        [DisplayName("Address")]
        public string Address { get; set; } // Optional address


        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth > DateTime.Now)
            {
                return new ValidationResult("Date of birth cannot be a future date.");
            }

            return ValidationResult.Success;
        }
    }
}