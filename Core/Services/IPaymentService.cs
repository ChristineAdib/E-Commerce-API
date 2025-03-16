using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPaymentService
    {
        Task<Cart?> CreateOrUpdatePaymentIntent(string CartId);
    }
}
