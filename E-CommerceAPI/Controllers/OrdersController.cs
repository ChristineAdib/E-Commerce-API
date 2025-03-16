using AutoMapper;
using Core;
using Core.Entities.Order_Aggregate;
using Core.Services;
using Core.Specifications;
using Core.Specifications.OrderSpec;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Errors;
using E_CommerceAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceAPI.Controllers
{ 
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper,IUnitOfWork unitOfWork)
        {
            this._orderService = orderService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<ShippingAddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.CartId, orderDto.DeliveryMethodId, MappedAddress);
            if (order is null) return BadRequest(new ApiResponse(400, "Ther is the problem in your order"));
            return Ok(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.ViewOrderHistoryForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "Ther is no orders for this user"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allOrders")]
        public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetAllOrders([FromQuery] OrderSpecParams orderParams)
        {
            var orders = await _orderService.GetAllOrdersForAllUsersAsync(orderParams);

            if (orders == null || !orders.Any())
                return NotFound(new ApiResponse(404, "There are no orders matching the criteria."));

            var Count = new OrderForCountAsync(orderParams);
            var totalOrders = await _unitOfWork.Repository<Order>().GetCountWithSpecAsync(Count);
            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(new Pagination<OrderToReturnDto>(orderParams.PageSize, orderParams.PageIndex, mappedOrders, totalOrders));
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("dailyOrders")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetDailyOrders()
        {
            var Orders = await _orderService.GenerateDailyReportAsync();

            if (Orders is null || !Orders.Any())
                return NotFound(new ApiResponse(404, "No orders found for today."));

            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(mappedOrders);
        }
    }
}
