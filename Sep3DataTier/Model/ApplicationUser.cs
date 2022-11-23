using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Model;

public class ApplicationUser : IdentityUser
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email { get; set; }


    public ApplicationUser(string email, string username)
    {
        Email = email;
        UserName = username;
    }

    public ApplicationUser()
    {
        
    }
}