using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IResponseCacheService
    {
        Task CachResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime);
        Task<string?> GetCachedResponse(string CacheKey);
    }
}
