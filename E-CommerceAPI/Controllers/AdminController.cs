using Core.Entities.Identity;
using Core.Services;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Identity;

namespace E_CommerceAPI.Controllers
{
    public class AdminController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AdminController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("AdminLogin")]
        public async Task<ActionResult<UserDto>> AdminLogin(LoginDto model)
        {
            var admin = await _userManager.FindByEmailAsync(model.Email);
            if (admin == null || !(await _userManager.IsInRoleAsync(admin, "Admin")))
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(admin, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = admin.DisplayName,
                Email = admin.Email,
                Token = await _tokenService.CreateTokenAsync(admin, _userManager)
            });
        }
    }
}
