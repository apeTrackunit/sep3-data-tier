using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IUserDao userDao;

    public AuthService(IUserDao userDao)
    {
        this.userDao = userDao;
    }
    
    public override async Task<UserOutput> Register(RegisterUserInput request, ServerCallContext context)
    {
        ApplicationUser userToCreate = new ApplicationUser(request.Email, request.Username);
        ApplicationUser user = await userDao.RegisterUserAsync(userToCreate, request.Password);
        
        var userRole = await userDao.GetUserRoleAsync(user);
        
        return await Task.FromResult(new UserOutput
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Password = user.PasswordHash,
            Role = userRole
        });
    }
    
    
    public override async Task<UserOutput> LoginUser(LoginUserInput request, ServerCallContext context)
    {
        ApplicationUser user = await userDao.GetUserByEmailAsync(request.Email);

        bool userLoggingIn = await userDao.LoginUser(user,request.Password);
        
        if (!userLoggingIn)
        {
            throw new Exception("Wrong credentials");
        }

        var userRole = await userDao.GetUserRoleAsync(user);

        return await Task.FromResult(new UserOutput
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Password = user.PasswordHash,
            Role = userRole
        });
    }

    public override async Task<UserOutput> GetUserByEmail(GetUserByEmailInput request, ServerCallContext context)
    {
        ApplicationUser user = await userDao.GetUserByEmailAsync(request.Email);

        var userRole = await userDao.GetUserRoleAsync(user);

        return await Task.FromResult(new UserOutput
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Password = user.PasswordHash,
            Role = userRole
        });
    }
}