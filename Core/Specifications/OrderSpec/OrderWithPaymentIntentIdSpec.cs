using Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentIdSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentIdSpec(string PaymentIntentId):base(o=>o.PaymentIntentId==PaymentIntentId)
        {
            
        }
    }
}
