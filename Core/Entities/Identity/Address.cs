using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }//fullname
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string AppUserId { get; set; }
        public AppUser User { get; set; }
    }
}
