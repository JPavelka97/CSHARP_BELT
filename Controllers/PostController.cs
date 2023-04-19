using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using CSHARP_BELT.Models;

namespace CSHARP_BELT.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private MyContext _context;

    public PostController(ILogger<PostController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }


    // ? __________________________CRUD - CREATE____________________

    // ! CREATE - CREATE POST - GET
    [SessionCheck]
    [HttpGet("posts/new")]
    public IActionResult NewPost()
    {
        return View("NewPost");
    }

    // ! CREATE - CREATE POST - POST
    [SessionCheck]
    [HttpPost("posts/create")]
    public IActionResult CreatePost(Post newPost)
    {
        // ? Pull CreatorID from Session
        newPost.CreatorId = (int)HttpContext.Session.GetInt32("UserId");
        newPost.CreatorName = (string)HttpContext.Session.GetString("Name");
        Console.WriteLine(newPost.CreatorName);

        // ? Validations
        if (ModelState.IsValid)
        {
            _context.Add(newPost);
            _context.SaveChanges();

            return View("ReadPost", newPost);
        }
        else
        {
            return NewPost();
        }
    }

    // ! CREATE - CREATE MANY TO MANY - POST
    [SessionCheck]
    [HttpPost("posts/{like_id}/addlike")]
    public IActionResult AddLike(Like newLike, int like_id)
    {
        Like? LikeToAdd = _context.Likes.FirstOrDefault(
            a => a.PostId == like_id && a.UserId == (int)HttpContext.Session.GetInt32("UserId")
        );

        if (LikeToAdd != null)
        {
            return RedirectToAction("Index", "Home", null);
        }

        newLike.UserId = (int)HttpContext.Session.GetInt32("UserId");
        newLike.PostId = like_id;
        _context.Add(newLike);
        _context.SaveChanges();
        return RedirectToAction("Dashboard", "Home", null);
    }

    [SessionCheck]
    [HttpPost("posts/{like_id}/removelike")]
    public IActionResult RemoveLike(Post oldPost, int like_id)
    {
        Like? LikeToDelete = _context.Likes.FirstOrDefault(
            a => a.PostId == like_id && a.UserId == (int)HttpContext.Session.GetInt32("UserId")
        );
        if (LikeToDelete == null)
        {
            return RedirectToAction("Index", "Home", null);
        }
        _context.Likes.Remove(LikeToDelete);
        _context.SaveChanges();
        return RedirectToAction("Dashboard", "Home", null);
    }


    // ? __________________________CRUD - READ____________________

    // ! READ - READ ONE POST - GET
    [SessionCheck]
    [HttpGet("posts/{id}")]
    public IActionResult ReadPost(int id)
    {
        Post? OnePost = _context.Posts
            .Include(l => l.Likes)
            .ThenInclude(u => u.User)
            .FirstOrDefault(p => p.PostId == id);
        return View("ReadPost", OnePost);
    }

    // ? ____________________________CRUD - UPDATE________________________

    // ! UPDATE - UPDATE ONE POST - GET
    [SessionCheck]
    [HttpGet("posts/{id}/edit")]
    public IActionResult EditPost(int id)
    {
        Post? PostToEdit = _context.Posts.FirstOrDefault(d => d.PostId == id);
        if (PostToEdit == null)
        {
            return RedirectToAction("Index", "Home", null);
        }
        return View("EditPost", PostToEdit);
    }

    // ! UPDATE - UPDATE ONE POST - POST
    [SessionCheck]
    [HttpPost("posts/{id}/update")]
    public IActionResult UpdatePost(Post newPost, int id)
    {
        Post? OldPost = _context.Posts.FirstOrDefault(d => d.PostId == id);
        if (ModelState.IsValid)
        {
            OldPost.ImageURL = newPost.ImageURL;
            OldPost.Title = newPost.Title;
            OldPost.Medium = newPost.Medium;
            OldPost.ForSale = newPost.ForSale;
            OldPost.UpdatedAt = newPost.UpdatedAt;
            _context.SaveChanges();

            return View("ReadPost", newPost);
        }
        else
        {
            return View("EditPost", OldPost);
        }
    }

        // ? ____________________________CRUD - DELETE_____________________

    // ! DELETE - DELETE ONE POST - POST
    [SessionCheck]
    [HttpPost("posts/{id}/destroy")]
    public IActionResult DestroyPost(int id)
    {
        Post? PostToDelete = _context.Posts.FirstOrDefault(a => a.PostId == id);
        if (PostToDelete == null)
        {
            return RedirectToAction("Index", "Home", null);
        }
        _context.Posts.Remove(PostToDelete);
        _context.SaveChanges();
        return RedirectToAction("Dashboard", "Home", null);
    }
}