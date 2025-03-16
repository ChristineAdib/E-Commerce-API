using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {

        }
        public ProductItemOrdered(int productId, string productName, string imageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
    }
}
