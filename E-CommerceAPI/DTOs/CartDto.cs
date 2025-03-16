using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPI.DTOs
{
    public class CartDto
    {
        [Required]
        public string Id { get; set; }
        public List<CartItemDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
