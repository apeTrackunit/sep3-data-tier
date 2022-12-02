using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using Model;
using NuGet.Protocol;
using Sep3DataTier.Database;

namespace Sep3DataTier.Repository;

using Microsoft.EntityFrameworkCore;

public class UserDao : IUserDao
{
    private readonly DatabaseContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public UserDao(DatabaseContext context, UserManager<ApplicationUser> userManager)
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

    public async Task<bool> LoginUser(ApplicationUser user, string requestPassword)
    {
        var result = await userManager.CheckPasswordAsync(user, requestPassword);

        if (!result)
        {
            throw new Exception("Incorrect credentials");
        }

        return true;
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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

    public async Task<ApplicationUser> GetUserByIdAsync(String id)
    {
        try
        {
            var result = await userManager.FindByIdAsync(id);

            if (result == null)
                throw new Exception("User was not found");

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

}
