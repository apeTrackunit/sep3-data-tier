using Model;
using Sep3DataTier.Database;

namespace Sep3DataTier.Repository;

public class UserEfcDao : IUserEfcDao
{
    private readonly DatabaseContext context;

    public UserEfcDao(DatabaseContext context)
    {
        this.context = context;
    }

    public Task<bool> RegisterUserAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}