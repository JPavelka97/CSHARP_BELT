using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using CSHARP_BELT.Models;

namespace CSHARP_BELT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    // ! INDEX (Login and Register Page)
    public IActionResult Index()
    {
        return View();
    }

    // ? ________________________DASHBOARD_____________________

    // ! DASHBOARD - DISPLAY ALL - GET
    [SessionCheck]
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        // ? Create a LIST of ALL POSTS from DATABASE
        List<Post> AllPosts = _context.Posts
            .Include(l => l.Likes)
            .ThenInclude(u => u.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

        return View("Dashboard", AllPosts);
    }










    // ? ___________________________LOGOUT__________________________

    // ! LOGOUT -LOGOUT USER - POST
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}

// ! SESSION CHECK
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
