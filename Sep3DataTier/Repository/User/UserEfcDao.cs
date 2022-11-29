using Microsoft.AspNetCore.Identity;
using Model;
using NuGet.Protocol;
using Sep3DataTier.Database;

namespace Sep3DataTier.Repository;

public class UserEfcDao : IUserEfcDao
{
    private readonly DatabaseContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public UserEfcDao(DatabaseContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public async Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password)
    {
        try
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            return result.Succeeded ? user : throw new Exception("User was not created " + result.ToJson());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<String?> GetUserRoleAsync(ApplicationUser user)
    {
        try
        {
            var result = await userManager.GetRolesAsync(user);

            return result.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }
}