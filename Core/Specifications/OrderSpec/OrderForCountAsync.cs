using Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpec
{
    public class OrderForCountAsync:BaseSpecifications<Order>
    {
        public OrderForCountAsync(OrderSpecParams orderParams) : base(o =>
            (string.IsNullOrEmpty(orderParams.Status) || string.Equals(o.Status.ToString(), orderParams.Status, StringComparison.OrdinalIgnoreCase)) &&
            (!orderParams.OrderId.HasValue || o.Id == orderParams.OrderId)
        )
        {
            
        }
    }
}
