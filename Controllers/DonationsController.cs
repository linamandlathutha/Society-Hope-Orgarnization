using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocietyHopeOrg.Data;
using SocietyHopeOrg.Models;
using Stripe;
using Stripe.Checkout;
using System.Net.Mail;
using System.Net;
using static SocietyHopeOrg.Models.Donation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;




namespace SocietyHopeOrg.Controllers
{
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonationsController> _logger;
        private readonly StripeSettings _stripeSettings;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConverter _converter;
        private readonly IRazorViewEngine _razorViewEngine; // Add this line
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory; // Add this for TempData


        public DonationsController(ApplicationDbContext context, ILogger<DonationsController> logger, IOptions<StripeSettings> stripeSettings, IConfiguration configuration, UserManager<IdentityUser> userManager, IConverter converter, IRazorViewEngine razorViewEngine, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _context = context;
            _logger = logger;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _configuration = configuration;
            _userManager = userManager;
            _converter = converter;
            _razorViewEngine = razorViewEngine; // Initialize the Razor view engine
            _tempDataDictionaryFactory = tempDataDictionaryFactory; // Initialize TempData factory

        }

     



        public IActionResult Invoice()
        { return View(); }


        // GET: Donations/Create
        [HttpGet]
        public async Task<IActionResult> CreateSession()
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
            var groceryDonation = new Donation();

            // If the user is not an admin, pre-fill the model with data from Profile
            if (!isAdmin)
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Email == userEmail);
                if (profile != null)
                {
                  
                    groceryDonation.DonorEmail = profile.Email;
                    
                }
            }
            ViewBag.PublishableKey = _stripeSettings.PublishableKey;
            return View(groceryDonation);
        }

        //// GET: Donation/Create
        //public IActionResult Create()
        //{
        //    ViewBag.PublishableKey = _stripeSettings.PublishableKey;
        //    return View();
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(Donation donation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the latest donation record to get current totals
                    var lastDonation = await _context.Donations
                        .OrderByDescending(d => d.DonationDate)
                        .FirstOrDefaultAsync();

                    // Calculate new totals
                    donation.TotalDonations = (lastDonation?.TotalDonations ?? 0) + donation.Amount;
                    donation.AvailableAmount = (lastDonation?.AvailableAmount ?? 0) + donation.Amount;
                    donation.DonationDate = DateTime.Now;
                    donation.Status = DonationStatus.Pending; // Set initial status

                    // Save the donation record
                    _context.Add(donation);
                    await _context.SaveChangesAsync();

                    // Create Stripe session
                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(donation.Amount * 100),
                            Currency = "zar",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Donation"
                            },
                        },
                        Quantity = 1,
                    },
                },
                        Mode = "payment",
                        SuccessUrl = Url.Action("Success", "Donations", new { id = donation.DonationId }, Request.Scheme),
                        CancelUrl = Url.Action("Cancel", "Donations", new { id = donation.DonationId }, Request.Scheme),
                    };

                    var service = new SessionService();
                    var session = service.Create(options);

                    return Redirect(session.Url);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error processing donation: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while processing your donation. Please try again.");
                }
            }

            return View(donation);
        }



        [HttpPost]
        public async Task<IActionResult> WithdrawAmount(int donationId, decimal withdrawalAmount, string notes)
        {
            var latestDonation = await _context.Donations.OrderByDescending(d => d.DonationDate).FirstOrDefaultAsync();

            if (latestDonation == null || latestDonation.DonationId != donationId)
            {
                return NotFound("Donation record not found or does not match the latest donation.");
            }

            if (withdrawalAmount <= 0 || withdrawalAmount > latestDonation.AvailableAmount)
            {
                TempData["WithdrawalError"] = "Withdrawal amount cannot exceed available funds.";
                return RedirectToAction("Index");
            }

            latestDonation.AvailableAmount -= withdrawalAmount;
            latestDonation.WithdrawnAmount = (latestDonation.WithdrawnAmount ?? 0) + withdrawalAmount;

            _context.Update(latestDonation);

            // Record the transaction
            var transaction = new WithdrawalTransaction
            {
                DonationId = latestDonation.DonationId,
                WithdrawnAmount = withdrawalAmount,
                AvailableBalanceAfter = latestDonation.AvailableAmount,
                WithdrawalDate = DateTime.Now,
                Notes = notes // Use the notes value passed from the form
            };

            _context.WithdrawalTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["WithdrawalSuccess"] = true;
            return RedirectToAction("Index");
        }






        public async Task<IActionResult> TransactionHistory(int donationId)
        {
            var donation = await _context.Donations
                                         .Include(d => d.WithdrawalTransactions)
                                         .FirstOrDefaultAsync(d => d.DonationId == donationId);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation.WithdrawalTransactions.OrderByDescending(t => t.WithdrawalDate).ToList());
        }



        // GET: User/Index
        public async Task<IActionResult> AllTransactions()
        {
            // Get all users
            var users = _context.WithdrawalTransactions;
            return View(await users.ToListAsync());
        }






        // This action will be called when the payment is successful
        public IActionResult Success(int id)
        {
            // Fetch the donation details from the database using the donation ID
            var donation = _context.Donations.Find(id); // Ensure you have the necessary logic for this

            if (donation == null)
            {
                return NotFound();
            }

            //// Send the confirmation email
            SendConfirmationEmail(donation);

            return View();
        }

       // Create a method to send the confirmation email
       private void SendConfirmationEmail(Donation donation)
        {
           // Get the email settings directly from IConfiguration
           var smtpServer = _configuration["EmailSettings:SmtpServer"];
           var smtpPort = _configuration["EmailSettings:SmtpPort"];
           var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = int.Parse(smtpPort),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            // Create a detailed email body
            string body = $@"
        <h2>Thank You for Your Donation!</h2>
        <p>Dear Donor,</p>
        <p>We sincerely appreciate your generous donation of <strong>{donation.Amount:C}</strong> made on <strong>{donation.DonationDate:yyyy-MM-dd}</strong>.</p>
        <p>Your support helps us continue our mission to make a positive impact in the community.</p>
        <p>Here are the details of your donation:</p>
        <ul>
            <li><strong>Amount:</strong> {donation.Amount:C}</li>
            <li><strong>Date of Donation:</strong> {donation.DonationDate:yyyy-MM-dd}</li>
            
            <li><strong>Message from Donor:</strong> {donation.DonorMessage}</li>
        </ul>
        <p>If you have any questions, feel free to contact us.</p>
        <p>Thank you once again for your support!</p>
        <p>Best Regards,<br>Your Organization</p>
    ";

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Donation Payment Confirmation",
                Body = body,
                IsBodyHtml = true,
            };

            // Add recipient(s)
            message.To.Add(donation.DonorEmail); // Send to the donor's email

            // Send the email
            smtpClient.Send(message);
        }










        public IActionResult Cancel()
        {
            return View();
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

                IQueryable<Donation> reservationsQuery = _context.Donations;

                if (isAdmin)
                {
                    // If there's a search string, filter by email
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        reservationsQuery = reservationsQuery.Where(d => d.DonorEmail.Contains(searchString));
                    }
                }
                else
                {
                    // For non-admin users, filter by the logged-in user's email and the search string if provided
                    reservationsQuery = reservationsQuery.Where(d => d.DonorEmail == userEmail);
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        reservationsQuery = reservationsQuery.Where(d => d.DonorEmail.Contains(searchString));
                    }
                }

                // Execute the query and convert to a list
                var reservations = await reservationsQuery.ToListAsync();
                return View(reservations);
            }
            catch (DbUpdateException dbEx)
            {
                // Log the exception and handle database-specific errors
                ModelState.AddModelError("", "There was an error accessing the database. Please try again later.");
                return RedirectToAction("Create", "Profile");
            }
            catch (Exception ex)
            {
                // Log the general exception and handle other errors
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return RedirectToAction("Create", "Profile");
            }
        }







        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }
    }
}
