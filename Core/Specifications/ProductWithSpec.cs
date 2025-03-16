using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Specifications
{
    public class ProductWithSpec : BaseSpecifications<Product>
    {
        public ProductWithSpec(ProductSpecParams Params)
            : base(P =>
            (!Params.CategoryId.HasValue || P.CategoryId == Params.CategoryId)
            &&
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
            )
        {
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
    }
}
