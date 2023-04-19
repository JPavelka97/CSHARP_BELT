using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using CSHARP_BELT.Models;

namespace CSHARP_BELT.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context;

    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }



    // ? ______________________LOGIN/REGISTER________________________

    // ! REGISTER - CREATE USER - POST
    [HttpPost("users/create")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("Name", newUser.Name);
            return RedirectToAction("Dashboard", "Home", null);
        }
        else
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }

    // ! LOGIN - LOGIN USER - POST
    [HttpPost("users/login")]
    public IActionResult Login(UserLogin userSubmit)
    {
        if (ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmit.EmailLogin);
            if (userInDb == null)
            {
                ModelState.AddModelError("EmailLogin", "Invalid Email/Password");
                return View("~/Views/Home/Index.cshtml");
            }
            PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin>();
            var result = hasher.VerifyHashedPassword(
                userSubmit,
                userInDb.Password,
                userSubmit.PasswordLogin
            );
            if (result == 0)
            {
                ModelState.AddModelError("PasswordLogin", "Invalid Email/Password");
                return View("~/Views/Home/Index.cshtml");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            HttpContext.Session.SetString("Name", userInDb.Name);
            return RedirectToAction("Dashboard", "Home", null);
        }
        else
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
