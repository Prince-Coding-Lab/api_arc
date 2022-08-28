using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    public class Orders : BaseEntity
    {
        public int? UserId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentMethod { get; set; }
        public int? PaymentId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Currency { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public int? StatusId { get; set; }
        public int? AddressType { get; set; }
        public List<OrderAds> OrderAds { get; set; }

    }
}
