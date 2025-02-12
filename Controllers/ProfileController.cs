using Microsoft.AspNetCore.Mvc;
using SocietyHopeOrg.Data;
using Microsoft.EntityFrameworkCore;
using SocietyHopeOrg.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;


namespace testsubject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;



        public ProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
            _configuration = configuration;
        }





        private string UploadedFile(ProfileViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }





        // GET: Profile/Create
        public IActionResult Create()
        {
            // Pass Google API key to the view
            ViewBag.GoogleApiKey = _configuration["GoogleAPI:PlacesApiKey"];
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(profile);
                // Check if an email already exists in the database
                var existingProfile = await _context.Profiles
                                                    .FirstOrDefaultAsync(p => p.Email == profile.Email);
                if (existingProfile != null)
                {
                    ModelState.AddModelError("Email", "A profile with this email address already exists.");
                    return View(profile);
                }
                Profile model = new Profile
                {
                    FirstName = profile.FirstName,
                    Surname = profile.Surname,
                    Email = profile.Email,
                    Address = profile.Address,
                    Gender = profile.Gender,
                    PhoneNumber= profile.PhoneNumber,
                    ProfilePicture = uniqueFileName,

                };

                _context.Profiles.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(profile);
        }




        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user's email
            var userEmail = User.Identity.Name;
            
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Email == userEmail);

            if (profile == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(profile); // Only showing the user's profile
        }







        // GET: Profile/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profile profile)
        {
            if (id != profile.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ProfileId == id);
        }
    }
}
