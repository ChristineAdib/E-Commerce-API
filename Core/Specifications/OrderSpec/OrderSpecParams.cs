using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.OrderSpec
{
    public class OrderSpecParams
    {
        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Status { get; set; } 
        public int? OrderId { get; set; } 
        public string? Sort { get; set; }
    }
}
