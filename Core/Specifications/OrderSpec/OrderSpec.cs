using Core.Entities.Order_Aggregate;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpec
{
    public class OrderSpec : BaseSpecifications<Order>
    {
        public OrderSpec(string email) : base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpec(OrderSpecParams orderParams)
        : base(o =>
            (string.IsNullOrEmpty(orderParams.Status) || string.Equals(o.Status.ToString(), orderParams.Status, StringComparison.OrdinalIgnoreCase)) &&
            (!orderParams.OrderId.HasValue || o.Id == orderParams.OrderId)
        )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(o => o.Items);
            Includes.Add(o => o.ShippingAddress);

            if (!string.IsNullOrEmpty(orderParams.Sort))
            {
                switch (orderParams.Sort.ToLower())
                {
                    case "price_asc":
                        AddOrderBy(o => o.SubTotal);
                        break;
                    case "price_desc":
                        AddOrderByDesc(o => o.SubTotal);
                        break;
                }
            }

            ApplyPagination(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }
    }
}
