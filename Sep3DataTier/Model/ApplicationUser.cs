using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Model;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Email { get; set; }
    [NotMapped]
    public virtual ICollection<Event> AttendedEvents { get; set; }

    public ApplicationUser(string email, string username)
    {
        Email = email;
        UserName = username;
    }
}