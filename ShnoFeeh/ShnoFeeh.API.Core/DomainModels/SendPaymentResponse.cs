using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Send Payment Response class.
    /// </summary>
    public class SendPaymentResponse
    {
        #region Properties
        public int InvoiceId { get; set; }
        public string InvoiceURL { get; set; }
        public string CustomerReference { get; set; }
        public string UserDefinedField { get; set; }
        #endregion
    }
}
