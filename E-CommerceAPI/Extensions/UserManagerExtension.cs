using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_CommerceAPI.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            return user;
        }
    }
}
