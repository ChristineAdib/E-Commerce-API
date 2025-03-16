using Core;
using Core.Entities;
using Core.Entities.Order_Aggregate;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepo _cartRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
            ICartRepo cartRepo,
            IUnitOfWork unitOfWork)
        {
            this._configuration = configuration;
            this._cartRepo = cartRepo;
            this._unitOfWork = unitOfWork;
        }
        public async Task<Cart?> CreateOrUpdatePaymentIntent(string CartId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var Cart = await _cartRepo.GetCartAsync(CartId);
            if (Cart is null) return null;
            var ShippingPrice = 0M;
            if (Cart.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Cart.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            if (Cart.Items.Count > 0)
            {
                foreach(var item in Cart.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = Cart.Items.Sum(item => item.Price * item.Quantity);

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Cart.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await Service.CreateAsync(Options);
                Cart.PaymentIntentId = paymentIntent.Id;
                Cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100
                };
                paymentIntent = await Service.UpdateAsync(Cart.PaymentIntentId, Options);
                Cart.PaymentIntentId = paymentIntent.Id;
                Cart.ClientSecret = paymentIntent.ClientSecret;
            }

            await _cartRepo.CreateOrUpdateAsync(Cart);
            return Cart;
        }
    }
}
