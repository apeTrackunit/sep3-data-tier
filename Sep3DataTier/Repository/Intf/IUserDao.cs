using Model;

namespace Sep3DataTier.Repository;

public interface IUserDao
{
    Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password, string role);
    Task<String?> GetUserRoleAsync(ApplicationUser user);
    Task<bool> LoginUserAsync(ApplicationUser user, string requestPassword);
    Task<ApplicationUser> GetUserByEmailAsync(string requestEmail);
}