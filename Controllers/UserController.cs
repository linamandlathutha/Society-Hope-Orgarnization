using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: User/Index
    public async Task<IActionResult> Index()
    {
        // Get all users
        var users = _userManager.Users;
        return View(await users.ToListAsync());
    }
}
