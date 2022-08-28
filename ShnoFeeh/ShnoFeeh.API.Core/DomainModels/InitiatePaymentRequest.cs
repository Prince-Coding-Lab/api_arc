using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Initiate Payment Request class.
    /// </summary>
    public class InitiatePaymentRequest
    {
        #region Properties
        public decimal InvoiceAmount { get; set; }
        public string CurrencyIso { get; set; }
        #endregion

    }
}
