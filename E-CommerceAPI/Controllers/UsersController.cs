using AutoMapper;
using Core.Entities.Identity;
using Core.Services;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using E_CommerceAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceAPI.Controllers
{
    public class UsersController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsersController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email is Already Exists"));
            }
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(User, model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto() {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }

        [Authorize]
        [HttpGet("UserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
        }

        [Authorize]
        [HttpPost("AddAddress")]
        public async Task<ActionResult> AddAddressAsync(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(address);
            user.Address = MappedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new { message = "Address Added successfully" });
        }

        [Authorize]
        [HttpPut("Updateaddress")]
        public async Task<ActionResult> UpdateAddressAsync(AddressDto address)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(address);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new { message = "Address Updated successfully" });
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
        }
    }
}
