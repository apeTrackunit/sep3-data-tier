using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Model;

public class ApplicationUser : IdentityUser
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }

    public ApplicationUser(string email, string username, string password)
    {
        Email = email;
        Username = username;
        Password = password;
    }

    public ApplicationUser()
    {
        
    }
}