using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Direct Payment Request class.
    /// </summary>
    public class DirectPaymentRequest
    {
        #region Properties
        public string PaymentType { get; set; }
        public bool SaveToken { get; set; }
        public bool IsRecurring { get; set; }
        public int IntervalDays { get; set; }
        public Card Card { get; set; }
        public string Token { get; set; }
        public bool Bypass3DS { get; set; }
        #endregion
    }
    /// <summary>
    /// Card class.
    /// </summary>
    public class Card
    {
        #region Properties
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string SecurityCode { get; set; }
        #endregion
    }
}
