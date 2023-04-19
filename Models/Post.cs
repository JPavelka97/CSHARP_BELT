#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSHARP_BELT.Models;


public class Post
{
    [Key]
    public int PostId {get;set;}

    [Required]
    public string ImageURL {get;set;}

    [Required]
    public string Title {get;set;}

    [Required]
    public string Medium {get;set;}

    [Required]
    public Boolean ForSale {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public int CreatorId {get;set;}
    public string? CreatorName {get;set;}
    public List<Like> Likes {get;set;} = new List<Like>();
}