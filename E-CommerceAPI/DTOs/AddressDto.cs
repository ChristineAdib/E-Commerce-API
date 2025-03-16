namespace E_CommerceAPI.DTOs
{
    public class AddressDto
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
    }
}
