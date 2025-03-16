using Core.Entities;
using System.Text.Json.Serialization;

namespace E_CommerceAPI.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryDTO>? Children { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
