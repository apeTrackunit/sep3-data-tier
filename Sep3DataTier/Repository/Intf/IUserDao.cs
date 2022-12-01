using Microsoft.AspNetCore.Mvc;
using Model;

namespace Sep3DataTier.Repository;

public interface IUserDao
{
    Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password);
    Task<String?> GetUserRoleAsync(ApplicationUser user);
    Task<bool> LoginUser(ApplicationUser user, string requestPassword);
    Task<ApplicationUser> GetUserByEmailAsync(string requestEmail);
    Task<ApplicationUser> GetUserByIdAsync(String id);
}       