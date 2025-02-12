namespace SocietyHopeOrg.Models
{
    public class InvoiceViewModel
    {
        public string OrganizationName { get; set; }
        public string DonorName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
        public string TransactionId { get; set; }
    }
}
