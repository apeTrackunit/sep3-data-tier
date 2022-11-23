using Microsoft.AspNetCore.Mvc;
using Model;

namespace Sep3DataTier.Repository;

public interface IUserEfcDao
{
    Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password);
}       