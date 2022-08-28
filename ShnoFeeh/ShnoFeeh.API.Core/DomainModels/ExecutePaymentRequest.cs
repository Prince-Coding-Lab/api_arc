using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Execute Payment Request class.
    /// </summary>
    public class ExecutePaymentRequest
    {
        #region Properties
        public int PaymentMethodId { get; set; }
        public string CustomerName { get; set; }
        public string DisplayCurrencyIso { get; set; }
        public string MobileCountryCode { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public decimal InvoiceValue { get; set; }
        public string CallBackUrl { get; set; }
        public string ErrorUrl { get; set; }
        public string Language { get; set; }
        public string CustomerReference { get; set; }
        public string CustomerCivilId { get; set; }
        public string UserDefinedField { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public string ExpiryDate { get; set; }
        public string SupplierCode { get; set; }
        public List<InvoiceItems> InvoiceItems { get; set; }
        #endregion

    }
}
