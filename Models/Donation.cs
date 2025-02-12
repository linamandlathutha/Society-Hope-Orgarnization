namespace SocietyHopeOrg.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        [DisplayName("Donor Email")]
        [Required]
        [EmailAddress]
        public string DonorEmail { get; set; } // Email of the donor (can be obtained from logged-in user)

        [DisplayName("Amount")]
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a valid donation amount.")]
        public decimal Amount { get; set; } // User-entered donation amount

        [DisplayName("Date of Donation")]
        public DateTime DonationDate { get; set; } = DateTime.Now; // Automatically set to current date

        [DisplayName("Status")]
        public DonationStatus Status { get; set; } = DonationStatus.Pending; // Default status as pending


        [DisplayName("Message from Donor")]
        [StringLength(500)]
        public string DonorMessage { get; set; } // Optional message from the donor


        // New Fields
        [DisplayName("Total Donations")]
        public decimal TotalDonations { get; set; } // Cumulative total of all donations

        [DisplayName("Available Amount")]
        public decimal AvailableAmount { get; set; } // Total donations minus any withdrawals

        [DisplayName("Withdrawn Amount")]
        public decimal? WithdrawnAmount { get; set; } // Amount withdrawn in specific cases, nullable



        // New: List of withdrawal transactions associated with this donation
        public List<WithdrawalTransaction> WithdrawalTransactions { get; set; } = new List<WithdrawalTransaction>();
        // Enum to track the donation status
        public enum DonationStatus
        {
            Pending,
            Completed,
            Failed
        }
    }

}
