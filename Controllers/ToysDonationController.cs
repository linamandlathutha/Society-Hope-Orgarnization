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
    public class ToysDonationController : Controller
    {
       
            private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ToysDonationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

                IQueryable<ToyDonation> toysQuery = _context.ToyDonations;

                if (isAdmin)
                {
                    // If there's a search string, filter by email
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        toysQuery = toysQuery.Where(d => d.Email.Contains(searchString));
                    }
                }
                else
                {
                    // For non-admin users, filter by the logged-in user's email and search string if provided
                    toysQuery = toysQuery.Where(d => d.Email == userEmail);
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        toysQuery = toysQuery.Where(d => d.Email.Contains(searchString));
                    }
                }

                var toys = await toysQuery.ToListAsync();
                return View(toys);
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



        // GET: GroceryDonation/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Get the logged-in user's email
            var userEmail = User.Identity.Name;

            // Retrieve the user by email
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return Unauthorized(); // Handle cases where the user is not found
            }

            // Check if the user has the "Admin" claim
            bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

            // Create a new GroceryDonation model
            var groceryDonation = new ToyDonation();

            // If the user is not an admin, pre-fill the model with data from Profile
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
        public IActionResult CreateAdmin(ToyDonation donation)
        {
            if (ModelState.IsValid)
            {
                _context.ToyDonations.Add(donation);
                _context.SaveChanges(); // Adjust if using async
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }


        // POST: GroceryDonation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToyDonation groceryDonation)
        {
            if (ModelState.IsValid)
            {
                _context.ToyDonations.Add(groceryDonation);
                _context.SaveChanges();
                // Send email confirmation
                await SendEmailConfirmation(groceryDonation);
                return RedirectToAction(nameof(Index));
            }

          
            return View(groceryDonation);
        }

        private async Task SendEmailConfirmation(ToyDonation donation)
        {
            // Email configuration from appsettings.json
            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "societyhopeorganization@gmail.com";
            var smtpPassword = "nqhxmkypgetnbksf";
            var fromAddress = "societyhopeorganization@gmail.com";
            var toAddress = donation.Email;

            var subject = "Donation Confirmation";
            var body = $"Dear {donation.FirstName},\n\n" +
                       $"Thank you for your generous donation of Toys. " +
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

            var groceryDonation = await _context.ToyDonations.FindAsync(id);
            if (groceryDonation == null)
            {
                return NotFound();
            }

            // Create a view model with only the fields you want to edit
            var viewModel = new UpdateTrackViewModel
            {
                DonationId = groceryDonation.Id,
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
                    var donationToUpdate = await _context.ToyDonations.FindAsync(id);
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
                    if (!ClothingDonationExists(groceryDonation.DonationId))
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




        private async Task SendEmailWithTrackingCode(ToyDonation donation)
        {
            // Email configuration
            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "societyhopeorganization@gmail.com";
            var smtpPassword = "nqhxmkypgetnbksf"; // Secure your password
            var fromAddress = "societyhopeorganization@gmail.com";
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
        private bool ClothingDonationExists(int id)
        {
            return _context.ToyDonations.Any(e => e.Id == id);
        }



       



























    }

    
}
