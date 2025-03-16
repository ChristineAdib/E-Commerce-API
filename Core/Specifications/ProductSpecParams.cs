using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecParams
    {
        //string? Sort,string? Search,int? CategoryId
        public string? Sort { get; set; }
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int? CategoryId { get; set; }

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }
        public int PageIndex { get; set; } = 1;
    }
}
