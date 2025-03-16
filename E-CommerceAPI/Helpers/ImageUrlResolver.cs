using AutoMapper;
using Core.Entities;
using E_CommerceAPI.DTOs;

namespace E_CommerceAPI.Helpers
{
    public class ImageUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.ImageUrl}";
            }
            return string.Empty;
        }
    }
}
