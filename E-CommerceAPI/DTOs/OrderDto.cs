using Core.Entities.Order_Aggregate;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPI.DTOs
{
    public class OrderDto
    {
        [Required]
        public string CartId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}
