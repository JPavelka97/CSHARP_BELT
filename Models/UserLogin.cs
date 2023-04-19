#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CSHARP_BELT.Models;

public class UserLogin
{
    [Required]
    public string EmailLogin {get;set;}

    [Required]
    [DataType(DataType.Password)]
    public string PasswordLogin {get;set;}
}