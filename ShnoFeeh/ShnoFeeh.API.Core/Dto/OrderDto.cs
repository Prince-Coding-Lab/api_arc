using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Dto
{

    /*
      This class contains data transfer object
      properties for Add order functionality
   */
    public class AddOrderDto
    {
        #region Properties
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
        public int? CreatedBy { get; set; }
        #endregion
    }
    /*
      This class contains data transfer object
      properties for Update order functionality
   */
    public class UpdateOrderDto
    {
        #region Properties
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
        public int? ModifiedBy { get; set; }
        public int? OrderId { get; set; }
        #endregion
    }
    /*
      This class contains data transfer object
      properties for order functionality
   */
    public class OrderDto
    {
        #region Properties
        public int Id { get; set; }
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
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public int? StatusId { get; set; }
        public int? AddressType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public List<OrderAdsDto> OrderAds { get; set; }
        #endregion
    }
}
