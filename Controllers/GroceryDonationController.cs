using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietyHopeOrg.Data;
using SocietyHopeOrg.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace SocietyHopeOrg.Controllers
{
    public class GroceryDonationController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GroceryDonationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                // Get the currently logged-in user's email
                var userEmail = User.Identity.Name;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(); // Handle the case where the user is not logged in
                }

                // Check if the user is in the Admin role
                var isAdmin = User.IsInRole("Admin");

                IQueryable<GroceryDonation> groceriesQuery = _context.GroceryDonations;

                if (isAdmin)
                {
                    // If there's a search string, filter by email
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        groceriesQuery = groceriesQuery.Where(d => d.Email.Contains(searchString));
                    }
                }
                else
                {
                    // For non-admin users, filter by the logged-in user's email and search string if provided
                    groceriesQuery = groceriesQuery.Where(d => d.Email == userEmail);
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        groceriesQuery = groceriesQuery.Where(d => d.Email.Contains(searchString));
                    }
                }

                var groceries = await groceriesQuery.ToListAsync();
                return View(groceries);
            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError("", "There was an error accessing the database. Please try again later.");
                return RedirectToAction("Create", "Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return RedirectToAction("Create", "Profile");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UseFoodItems(FoodUsageViewModel usage)
        {
            if (ModelState.IsValid)
            {
                // Retrieve all donations
                var donations = await _context.GroceryDonations.ToListAsync();

                // Check if there are any donations
                if (donations.Any())
                {
                    // Loop through each food type
                    int totalCannedFoodUsed = 0;
                    int totalBeansUsed = 0;
                    int totalPastaOrRiceUsed = 0;
                    int totalSugarFlourSaltCondimentsUsed = 0;
                    int totalBabyProductsUsed = 0;

                    // Deduct from donations
                    foreach (var donation in donations)
                    {
                        if (totalCannedFoodUsed < usage.CannedFoodQuantity)
                        {
                            // Assuming CannedFoodQuantity is of type int
                            int availableToUse = donation.CannedFoodQuantity; // No need for ?? here
                            int quantityToUse = Math.Min((byte)availableToUse, (byte)(usage.CannedFoodQuantity - totalCannedFoodUsed));
                            donation.CannedFoodQuantity -= quantityToUse;
                            totalCannedFoodUsed += quantityToUse;
                        }

                        if (totalBeansUsed < usage.BeansQuantity)
                        {
                            int availableToUse = donation.BeansQuantity; // No need for ?? here
                            int quantityToUse = Math.Min((byte)availableToUse, (byte)(usage.BeansQuantity - totalBeansUsed));
                            donation.BeansQuantity -= quantityToUse;
                            totalBeansUsed += quantityToUse;
                        }

                        if (totalPastaOrRiceUsed < usage.PastaOrRiceQuantity)
                        {
                            int availableToUse = donation.PastaOrRiceQuantity; // No need for ?? here
                            int quantityToUse = Math.Min((byte)availableToUse, (byte)(usage.PastaOrRiceQuantity - totalPastaOrRiceUsed));
                            donation.PastaOrRiceQuantity -= quantityToUse;
                            totalPastaOrRiceUsed += quantityToUse;
                        }

                        if (totalSugarFlourSaltCondimentsUsed < usage.SugarFlourSaltCondimentsQuantity)
                        {
                            int availableToUse = donation.SugarFlourSaltCondimentsQuantity; // No need for ?? here
                            int quantityToUse = Math.Min((byte)availableToUse, (byte)(usage.SugarFlourSaltCondimentsQuantity - totalSugarFlourSaltCondimentsUsed));
                            donation.SugarFlourSaltCondimentsQuantity -= quantityToUse;
                            totalSugarFlourSaltCondimentsUsed += quantityToUse;
                        }

                        if (totalBabyProductsUsed < usage.BabyProductsQuantity)
                        {
                            int availableToUse = donation.BabyProductsQuantity; // No need for ?? here
                            int quantityToUse = Math.Min((byte)availableToUse, (byte)(usage.BabyProductsQuantity - totalBabyProductsUsed));
                            donation.BabyProductsQuantity -= quantityToUse;
                            totalBabyProductsUsed += quantityToUse;
                        }

                        // Break if all requested quantities have been satisfied
                        if (totalCannedFoodUsed >= usage.CannedFoodQuantity &&
                            totalBeansUsed >= usage.BeansQuantity &&
                            totalPastaOrRiceUsed >= usage.PastaOrRiceQuantity &&
                            totalSugarFlourSaltCondimentsUsed >= usage.SugarFlourSaltCondimentsQuantity &&
                            totalBabyProductsUsed >= usage.BabyProductsQuantity)
                        {
                            break;
                        }
                    }


                    // Save updated quantities
                    await _context.SaveChangesAsync();

                    // Record the usage transaction
                    var transaction = new FoodUsageTransaction
                    {
                        CannedFoodQuantityUsed = totalCannedFoodUsed,
                        BeansQuantityUsed = totalBeansUsed,
                        PastaOrRiceQuantityUsed = totalPastaOrRiceUsed,
                        SugarFlourSaltCondimentsQuantityUsed = totalSugarFlourSaltCondimentsUsed,
                        BabyProductsQuantityUsed = totalBabyProductsUsed,
                        MealTime = usage.MealTime,
                        UsageDate = DateTime.Now
                    };
                    _context.FoodUsageTransactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Food items used successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No food donations available.";
                }

                return RedirectToAction("Index");
            }

            return View("Index", _context.GroceryDonations.ToList());
        }



























        // GET: GroceryDonation/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userEmail = User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");
            var groceryDonation = new GroceryDonation();

            if (!isAdmin)
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Email == userEmail);
                if (profile != null)
                {
                    groceryDonation.FirstName = profile.FirstName;
                    groceryDonation.Surname = profile.Surname;
                    groceryDonation.Email = profile.Email;
                    groceryDonation.PhoneNumber = profile.PhoneNumber;
                }
            }

            return View(groceryDonation);
        }





        // GET: ClothingDonation/Create
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: ClothingDonation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAdmin(GroceryDonation donationAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.GroceryDonations.Add(donationAdmin);
                _context.SaveChanges(); // Adjust if using async
                return RedirectToAction(nameof(Index));
            }
            return View(donationAdmin);
        }


        // POST: GroceryDonation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroceryDonation groceryDonation)
        {
            if (ModelState.IsValid)
            {
                _context.GroceryDonations.Add(groceryDonation);
                await _context.SaveChangesAsync();

                // Update the FoodInventory table
                var inventory = await _context.FoodInventories.FirstOrDefaultAsync();

                if (inventory != null)
                {
                    inventory.CannedFoods += groceryDonation.CannedFoodQuantity;
                    inventory.Beans += groceryDonation.BeansQuantity;
                    inventory.PastaOrRice += groceryDonation.PastaOrRiceQuantity;
                    inventory.SugarFlourSaltCondiments += groceryDonation.SugarFlourSaltCondimentsQuantity;
                    inventory.BabyProducts += groceryDonation.BabyProductsQuantity;

                    _context.FoodInventories.Update(inventory);
                    await _context.SaveChangesAsync();
                }

               //  Send email confirmation
                await SendEmailConfirmation(groceryDonation);
                return RedirectToAction(nameof(Index));
            }

            return View(groceryDonation);
        }



        // GET: FoodUsageTransaction/History
        public async Task<IActionResult> History()
        {
            var transactions = await _context.FoodUsageTransactions
                                             .OrderByDescending(t => t.UsageDate)
                                             .ToListAsync();
            return View(transactions);
        }



        private async Task SendEmailConfirmation(GroceryDonation donation)
        {
            // Email configuration from appsettings.json
            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "youremail@gmail.com";
            var smtpPassword = "yourpassword";
            var fromAddress = "youremail@gmail.com";
            var toAddress = donation.Email;

            var subject = "Donation Confirmation";
            var body = $"Dear {donation.FirstName},\n\n" +
                       $"Thank you for your generous donation of Groceries. " +
                       $"We will arrange for a courier to pick up your items at your designated address.\n\n" +
                       "Best regards,\n" +
                       "The Donation Team";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (var message = new MailMessage(fromAddress, toAddress, subject, body))
                {
                    await client.SendMailAsync(message);
                }
            }
        }


        // GET: GroceryDonation/EditTrackingCode/5
        public async Task<IActionResult> EditTrackingCode(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groceryDonation = await _context.GroceryDonations.FindAsync(id);
            if (groceryDonation == null)
            {
                return NotFound();
            }

            // Create a view model with only the fields you want to edit
            var viewModel = new UpdateTrackViewModel
            {
                DonationId = groceryDonation.DonationId,
                Tracking = groceryDonation.TrackingCode,
            };

            return View(viewModel);
        }

        // POST: GroceryDonation/EditTrackingCode/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrackingCode(int id, UpdateTrackViewModel groceryDonation)
        {
            if (id != groceryDonation.DonationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var donationToUpdate = await _context.GroceryDonations.FindAsync(id);
                    if (donationToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Update only the TrackingCode
                    donationToUpdate.TrackingCode = groceryDonation.Tracking;

                    await _context.SaveChangesAsync();

                    // Send email notification
                    await SendEmailWithTrackingCode(donationToUpdate);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroceryDonationExists(groceryDonation.DonationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(groceryDonation);
        }

        private async Task SendEmailWithTrackingCode(GroceryDonation donation)
        {
            // Email configuration
            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "youremail@gmail.com";
            var smtpPassword = "yourpassword"; // Secure your password
            var fromAddress = "youremail@gmail.com";
            var toAddress = donation.Email; // Assuming the donation has an Email property

            var subject = "Your Tracking Code Update";
            var body = $"Dear {donation.FirstName},\n\n" +
               $"Your tracking code has been updated to: **{donation.TrackingCode}**.\n\n" +
               $"You can use your tracking number on our tracking website: " +
               $"[Track Your Donation](https://thecourierguy.co.za/tracking).\n\n" + // Update this URL
               "Thank you for your continued support!\n" +
               "Best regards,\n" +
               "The Donation Team";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (var message = new MailMessage(fromAddress, toAddress, subject, body))
                {
                    await client.SendMailAsync(message);
                }
            }
        }

        private bool GroceryDonationExists(int id)
        {
            return _context.GroceryDonations.Any(e => e.DonationId == id);
        }
    }
}
