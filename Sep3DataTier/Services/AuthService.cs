using Grpc.Core;
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

        return await Task.FromResult(new RegisterUserOutput
        {
            Email = user.Email,
            Id = user.Id,
            Password = user.PasswordHash,
            Username = user.UserName
        });
    }
}