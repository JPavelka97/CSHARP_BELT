#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSHARP_BELT.Models;

public class Like
{
    [Key]
    public int LikeId {get;set;}

    public int UserId {get;set;}

    public int PostId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public Post? Post {get;set;}
    public User? User {get;set;}
}