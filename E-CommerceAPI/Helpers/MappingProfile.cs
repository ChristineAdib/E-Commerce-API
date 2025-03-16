using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Order_Aggregate;
using E_CommerceAPI.DTOs;

namespace E_CommerceAPI.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.ImageUrl, o => o.MapFrom<ImageUrlResolver>());

            CreateMap<Category, CategoryDTO>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Products))
                .ForMember(d => d.Children, o => o.MapFrom(s => s.Children));

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CartDto, Cart>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<ShippingAddressDto, Core.Entities.Order_Aggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()))
                .ForMember(d => d.PaymentIntentId, o => o.MapFrom(s => s.PaymentIntentId));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product.ImageUrl))
                .ForMember(d => d.ImageUrl, o => o.MapFrom<OrderItemImageUrlResolver>());

            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId));
        }
    }
}
