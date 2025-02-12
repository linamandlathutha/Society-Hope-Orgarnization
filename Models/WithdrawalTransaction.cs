using SocietyHopeOrg.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WithdrawalTransaction
{
    [Key]
    public int TransactionId { get; set; }

    [Required]
    public int DonationId { get; set; } // Reference to the donation record

    [ForeignKey("DonationId")]
    public Donation Donation { get; set; }

    [Required]
    [Display(Name = "Amount Withdrawn")]
    [Range(0.01, double.MaxValue, ErrorMessage = "The withdrawn amount must be greater than zero.")]
    public decimal WithdrawnAmount { get; set; } // The amount withdrawn for this transaction

    [Required]
    [Display(Name = "Available Balance After Withdrawal")]
    public decimal AvailableBalanceAfter { get; set; } // Available balance remaining after the withdrawal

    [Required]
    [Display(Name = "Date of Withdrawal")]
    public DateTime WithdrawalDate { get; set; } = DateTime.Now; // Timestamp of the withdrawal

    [StringLength(500)]
    public string Notes { get; set; } // Optional notes for further details about the transaction
}
