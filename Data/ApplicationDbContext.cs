using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocietyHopeOrg.Models;

namespace SocietyHopeOrg.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<WithdrawalTransaction> WithdrawalTransactions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<GroceryDonation> GroceryDonations { get; set; }
        public DbSet<FoodInventory> FoodInventories { get; set; }
        public DbSet<FoodUsageTransaction> FoodUsageTransactions { get; set; }
        public DbSet<ClothingDonation> ClothingDonations { get; set; }
        public DbSet<ToyDonation> ToyDonations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
