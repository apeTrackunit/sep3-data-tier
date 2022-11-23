using Grpc.Core;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IUserEfcDao userEfcDao;

    public async override Task<RegisterUserOutput> Register(RegisterUserInput request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}