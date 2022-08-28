using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Services
{
    /// <summary>
    /// This class is for Payment Service.
    /// It implements all methods of IPaymentService.
    /// </summary>
    /// <remarks>
    /// This class have sendpayment and getpaymentstatus methods.
    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientHelper _paymentRepository;
        private readonly ICommon _common;
        public PaymentService(IHttpClientHelper paymentRepository,
           ICommon common)
        {
            _paymentRepository = paymentRepository;
            _common = common;
        }

        public async Task<ResponseModel<PaymentStatusResponse>> GetPaymentStatusAsync(PaymentStatusRequest status)
        {
            return await _paymentRepository.PostRequest<PaymentStatusRequest, ResponseModel<PaymentStatusResponse>>(string.Format(_common.GetUri(ApiUris.GetPaymentStatusUrl)), status);
        }

        public async Task<ResponseModel<SendPaymentResponse>> SendPaymentAsync(SendPaymentRequest payment)
        {
            return await _paymentRepository.PostRequest<SendPaymentRequest, ResponseModel<SendPaymentResponse>>(string.Format(_common.GetUri(ApiUris.SendPaymentUrl)), payment);
        }
        public async Task<ResponseModel<ExecutePaymentResponse>> ExecutePaymentAsync(ExecutePaymentRequest payment)
        {
            return await _paymentRepository.PostRequest<ExecutePaymentRequest, ResponseModel<ExecutePaymentResponse>>(string.Format(_common.GetUri(ApiUris.ExecutePaymentUrl)), payment);
        }

        public async Task<ResponseModel<InitiatePaymentResponse>> InitiatePaymentAsync(InitiatePaymentRequest initPayment)
        {
            return await _paymentRepository.PostRequest<InitiatePaymentRequest, ResponseModel<InitiatePaymentResponse>>(string.Format(_common.GetUri(ApiUris.InitiatePaymentUrl)), initPayment);
        }
        public async Task<ResponseModel<DirectPaymentResponse>> DirectPaymentAsync(DirectPaymentRequest payment, string invoiceKey, int paymentGatewayId)
        {
            return await _paymentRepository.PostRequest<DirectPaymentRequest, ResponseModel<DirectPaymentResponse>>(string.Format(_common.GetUri(ApiUris.DirectPaymentUrl), invoiceKey, paymentGatewayId), payment);
        }
    }
}
