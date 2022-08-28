using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Direct Payment Response class.
    /// </summary>
    public class DirectPaymentResponse
    {
        #region Properties
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string PaymentId { get; set; }
        public string Token { get; set; }
        public string RecurringId { get; set; }
        public string PaymentURL { get; set; }
        public CardInfo CardInfo { get; set; }
        #endregion

    }
    /// <summary>
    /// Card Info class.
    /// </summary>
    public class CardInfo
    {
        #region Properties
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Brand { get; set; }
        public string Issuer { get; set; }
        #endregion
    }
}
