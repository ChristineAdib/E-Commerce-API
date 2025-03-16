using Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpec
{
    public class DailyOrdersSpecification:BaseSpecifications<Order>
    {
        public DailyOrdersSpecification(DateTime date)
           : base(o => o.OrderDate.Date == date.Date)
        {
            Includes.Add(o => o.Items); 
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.ShippingAddress);
        }
    }
}
