using System;

namespace ShnoFeeh.API.Core.Dto
{
    /*
       This class contains data transfer object
       properties for PaymentReference functionality
   */
    public class PaymentReferenceDto
    {
        #region Properties
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion
    }
    /*
        This class contains data transfer object
        properties for Add PaymentReference functionality
    */
    public class AddPaymentReferenceDto
    {
        #region Properties
        public string ExternalId { get; set; }
        public int? UserId { get; set; }
        #endregion
    }
}
