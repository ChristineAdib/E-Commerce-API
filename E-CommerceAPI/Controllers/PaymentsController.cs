using Core.Services;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace E_CommerceAPI.Controllers
{
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }
        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateOrUpdatePaymentIntent(string CartId)
        {
            var Cart = await _paymentService.CreateOrUpdatePaymentIntent(CartId);
            if (CartId is null) return BadRequest(new ApiResponse(400));
            return Ok(Cart);
        }
    }
}
