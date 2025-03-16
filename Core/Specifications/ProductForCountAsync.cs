using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductForCountAsync : BaseSpecifications<Product>
    {
        public ProductForCountAsync(ProductSpecParams Params)
            : base(P =>
            (!Params.CategoryId.HasValue || P.CategoryId == Params.CategoryId)
            &&
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search.ToLower()))
            )
        {
            
        }
    }
}
