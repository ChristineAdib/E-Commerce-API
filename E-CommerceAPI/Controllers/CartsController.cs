using AutoMapper;
using Core.Entities;
using Core.Repositories;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceAPI.Controllers
{
    public class CartsController : APIBaseController
    {
        private readonly ICartRepo _cartRepo;
        private readonly IMapper _mapper;

        public CartsController(ICartRepo cartRepo,IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetCart(string CartId)
        {
            var Cart = await _cartRepo.GetCartAsync(CartId);
            return Cart is null ? new Cart(CartId) : Ok(Cart);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> CreateOrUpdateCart(CartDto Cart)
        {
            var MappedCart = _mapper.Map<CartDto, Cart>(Cart);
            var CreatedOrUpdatedCart = await _cartRepo.CreateOrUpdateAsync(MappedCart);
            if (CreatedOrUpdatedCart is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DelecteCart(string CartId)
        {
            return await _cartRepo.DeleteCartAsync(CartId);
        }
    }
}
