using ShnoFeeh.API.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseModel<SendPaymentResponse>> SendPaymentAsync(SendPaymentRequest payment);
        Task<ResponseModel<PaymentStatusResponse>> GetPaymentStatusAsync(PaymentStatusRequest status);
        Task<ResponseModel<InitiatePaymentResponse>> InitiatePaymentAsync(InitiatePaymentRequest initPayment);
        Task<ResponseModel<ExecutePaymentResponse>> ExecutePaymentAsync(ExecutePaymentRequest payment);
        Task<ResponseModel<DirectPaymentResponse>> DirectPaymentAsync(DirectPaymentRequest payment, string invoiceKey, int paymentGatewayId);

    }
}
