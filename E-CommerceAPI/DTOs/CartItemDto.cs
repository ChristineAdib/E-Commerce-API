using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPI.DTOs
{
    public class CartItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Can Not Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage = "Quantity Must Be One Item At Least")]
        public int Quantity { get; set; }
    }
}
