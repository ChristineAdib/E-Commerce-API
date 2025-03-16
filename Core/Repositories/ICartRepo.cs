using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface ICartRepo
    {
        Task<Cart?> GetCartAsync(string CartId);
        Task<Cart?> CreateOrUpdateAsync(Cart Cart);
        Task<bool> DeleteCartAsync(string CartId);
    }
}