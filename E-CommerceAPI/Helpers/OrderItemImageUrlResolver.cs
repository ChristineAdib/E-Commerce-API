using AutoMapper;
using Core.Entities.Order_Aggregate;
using E_CommerceAPI.DTOs;

namespace E_CommerceAPI.Helpers
{
    public class OrderItemImageUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemImageUrlResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.ImageUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.ImageUrl}";
            }
            return string.Empty;
        }
    }
}
