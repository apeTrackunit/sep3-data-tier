using Microsoft.AspNetCore.Identity;
using Model;
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

            return result.Succeeded ? user : throw new Exception("User was not created");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}