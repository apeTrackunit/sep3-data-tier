using Grpc.Core;
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
        ApplicationUser userToReturn = await userDao.RegisterUserAsync(userToCreate, request.Password, request.Role);

        var userRole = await userDao.GetUserRoleAsync(userToReturn);
        
        return await Task.FromResult(new UserOutput
        {
            Id = userToReturn.Id,
            Username = userToReturn.UserName,
            Email = userToReturn.Email,
            Password = userToReturn.PasswordHash,
            Role = userRole
        });
    }
    
    
    public override async Task<UserOutput> LoginUser(LoginUserInput request, ServerCallContext context)
    {
        ApplicationUser user = await userDao.GetUserByEmailAsync(request.Email);

        bool userLoggingIn = await userDao.LoginUserAsync(user,request.Password);
        
        if (!userLoggingIn)
            throw new Exception("Wrong credentials");

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