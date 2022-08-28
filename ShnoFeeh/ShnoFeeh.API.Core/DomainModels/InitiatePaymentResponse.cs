using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Initiate Payment Response class.
    /// </summary>
    public class InitiatePaymentResponse
    {
        #region Properties
        public int PaymentMethodId { get; set; }
        public int PaymentMethodAr { get; set; }
        public int PaymentMethodEn { get; set; }
        public bool IsDirectPayment { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalAmount { get; set; }
        public string CurrencyIso { get; set; }
        public string ImageUrl { get; set; }
        #endregion
    }
}
