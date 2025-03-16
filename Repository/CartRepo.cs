using Core.Entities;
using Core.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class CartRepo : ICartRepo
    {
        private readonly IDatabase _database;
        public CartRepo(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<Cart?> CreateOrUpdateAsync(Cart Cart)
        {
            var JsonCart = JsonSerializer.Serialize(Cart);
            var CreatedOrUpdated = await _database.StringSetAsync(Cart.Id, JsonCart, TimeSpan.FromDays(4));
            if (!CreatedOrUpdated) return null;
            return await GetCartAsync(Cart.Id);
        }

        public async Task<bool> DeleteCartAsync(string CartId)
        {
            return await _database.KeyDeleteAsync(CartId);
        }

        public async Task<Cart?> GetCartAsync(string CartId)
        {
            var Cart = await _database.StringGetAsync(CartId);
            return Cart.IsNull ? null : JsonSerializer.Deserialize<Cart>(Cart);
        }
    }
}
