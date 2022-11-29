using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IUserEfcDao userEfcDao;

    public AuthService(IUserEfcDao userEfcDao)
    {
        this.userEfcDao = userEfcDao;
    }
    
    public override async Task<RegisterUserOutput> Register(RegisterUserInput request, ServerCallContext context)
    {
        ApplicationUser userToCreate = new ApplicationUser(request.Email, request.Username);
        ApplicationUser user = await userEfcDao.RegisterUserAsync(userToCreate, request.Password);
        
        var userRole = await userEfcDao.GetUserRoleAsync(user);
        
        return await Task.FromResult(new RegisterUserOutput
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Password = user.PasswordHash,
            Role = userRole
        });
    }
}