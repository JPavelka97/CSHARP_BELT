#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSHARP_BELT.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MinLength(2)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    [ExtraPassword]
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Like> Likes { get; set; } = new List<Like>();

    [NotMapped]
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}

public class ExtraPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
    {
        Boolean checks = false;
        if (value == null){
            return new ValidationResult("Password must not be empty");            
        }
        string? passString = value.ToString();
        if (passString == null)
        {
            return new ValidationResult("Password must not be empty");
        }
        else
        {
            Console.WriteLine(passString);
            if (
                passString.Any(char.IsLetter)
                && passString.Any(char.IsNumber)! & passString.Any(char.IsLetterOrDigit)
            )
            {
                checks = true;
            }
            Console.WriteLine(checks);
            if (checks == true)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "Password must contain one symbol, one letter, and one number"
                );
            }
        }
    }
}
