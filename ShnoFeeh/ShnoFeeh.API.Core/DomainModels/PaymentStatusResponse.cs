using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Payment Status Response class.
    /// </summary>
    public class PaymentStatusResponse
    {
        #region Properties
        public int InvoiceId { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceReference { get; set; }
        public string CustomerReference { get; set; }
        public string CreatedDate { get; set; }
        public string ExpiryDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public string Comments { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string UserDefinedField { get; set; }
        public string InvoiceDisplayValue { get; set; }
        public List<InvoiceItems> InvoiceItems { get; set; }
        public List<InvoiceTransactions> InvoiceTransactions { get; set; }
        #endregion

    }
    /// <summary>
    /// Invoice Transactions class.
    /// </summary>
    public class InvoiceTransactions
    {
        #region Properties
        public string TransactionDate { get; set; }
        public string PaymentGateway { get; set; }
        public string ReferenceId { get; set; }
        public string TrackId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        public string AuthorizationId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransationValue { get; set; }
        public string CustomerServiceCharge { get; set; }
        public string DueValue { get; set; }
        public string PaidCurrency { get; set; }
        public string PaidCurrencyValue { get; set; }
        public string Currency { get; set; }
        public string Error { get; set; }
        public string CardNumber { get; set; }
        #endregion
    }
}
