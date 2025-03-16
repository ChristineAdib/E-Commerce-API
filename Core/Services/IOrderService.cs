using Core.Entities.Order_Aggregate;
using Core.Specifications.OrderSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string CartId, int DeliveryMethod, Address ShippingAddress);
        Task<IReadOnlyList<Order>> ViewOrderHistoryForSpecificUserAsync(string BuyerEmail);
        Task<IReadOnlyList<Order>> GetAllOrdersForAllUsersAsync(OrderSpecParams orderSpecParams);
        Task<IReadOnlyList<Order>> GenerateDailyReportAsync();
    }
}
