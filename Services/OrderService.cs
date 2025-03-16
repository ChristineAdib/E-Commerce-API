using Core;
using Core.Entities;
using Core.Entities.Order_Aggregate;
using Core.Repositories;
using Core.Services;
using Core.Specifications.OrderSpec;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(ICartRepo cartRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _cartRepo = cartRepo;
            this._unitOfWork = unitOfWork;
            this._paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string CartId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Cart = await _cartRepo.GetCartAsync(CartId);

            var OrderItems = new List<OrderItem>();
            if (Cart?.Items.Count > 0)
            {
                foreach(var item in Cart.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.ImageUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var Spec = new OrderWithPaymentIntentIdSpec(Cart.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(CartId);
            }

            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal,Cart.PaymentIntentId);

            await _unitOfWork.Repository<Order>().AddAsync(Order);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return Order;

        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersForAllUsersAsync(OrderSpecParams orderParams)
        {
            var spec = new OrderSpec(orderParams);
            return await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> ViewOrderHistoryForSpecificUserAsync(string BuyerEmail)
        {
            var Spec = new OrderSpec(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
        public async Task<IReadOnlyList<Order>> GenerateDailyReportAsync()
        {
            var spec = new DailyOrdersSpecification(DateTime.UtcNow);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
