﻿using Microsoft.AspNetCore.Mvc;
using Model;

namespace Sep3DataTier.Repository;

public interface IUserEfcDao
{
    Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password);
    Task<String?> GetUserRoleAsync(ApplicationUser user);
    Task<bool> LoginUser(ApplicationUser user, string requestPassword);
    Task<ApplicationUser> GetUserByEmailAsync(string requestEmail);
}       