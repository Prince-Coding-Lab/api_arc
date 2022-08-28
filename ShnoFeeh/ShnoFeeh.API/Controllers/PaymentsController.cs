using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Model;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        #region Fields
        private readonly IPaymentRefService _reference;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public PaymentsController(IPaymentService paymentService, IPaymentRefService reference,IConfiguration configuration)
        {
            _paymentService = paymentService;
            _reference = reference;
            _configuration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// This kind of payment will facilitate your collection if you have a non-store platforms
        /// and would like to introduce a niche to your business.
        /// SendPaymnt method is a offline payment.
        /// </summary>
        /// <param name="payment">Payment object</param>
        /// <returns>Api Response</returns>
        [HttpPost("SendPayment")]
        public async Task<IActionResult> PostAsync([FromBody] SendPaymentRequest payment,int? userId)
        {
            payment.SupplierCode = _configuration.GetSection("MyFatoorahKeys:SupplierCode").Value;

            var response = await _paymentService.SendPaymentAsync(payment);


            if (response.IsSuccess)
            {                
                PaymentReference createReference = new PaymentReference();
                createReference.ExternalId = Convert.ToString(response.Data.InvoiceId);
                createReference.UserId = userId;
                await _reference.AddAsync(createReference);
                return Ok(ApiResponse.OkResult(true, response.Data, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.ValidationErrors, DbReturnValue.CreationFailed));
            }
        }
        /// <summary>
        /// Payment Enquiry you can get the status of your invoice to assure 
        /// that the payment has been done successfully or not.
        /// </summary>
        /// <param name="status">Status request</param>
        /// <returns>Api Response</returns>
        [HttpPost("GetPaymentStatus")]
        public async Task<IActionResult> GetPaymentStatusAsync([FromBody] PaymentStatusRequest status)
        {
            var response = await _paymentService.GetPaymentStatusAsync(status);
            if (response.IsSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Data, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.ValidationErrors, DbReturnValue.CreationFailed));
            }
        }
        /// <summary>
        /// Execute Payment the actual payment execution step before proceeding 
        /// to the payment gateway for transaction processing
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpPost("ExecutePayment")]
        public async Task<IActionResult> ExecutePaymentAsync([FromBody] ExecutePaymentRequest payment, int? userId)
        {
            payment.SupplierCode = _configuration.GetSection("MyFatoorahKeys:SupplierCode").Value;

            var response = await _paymentService.ExecutePaymentAsync(payment);

            

            if (response.IsSuccess)
            {
                PaymentReference createReference = new PaymentReference();
                createReference.ExternalId = Convert.ToString(response.Data.InvoiceId);
                createReference.UserId = userId;
                await _reference.AddAsync(createReference);
                return Ok(ApiResponse.OkResult(true, response.Data, DbReturnValue.CreateSuccess));

            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.ValidationErrors, DbReturnValue.CreationFailed));
            }
        }

        /// <summary>
        /// Initiate Payment the preparation step that enables your system to collect all needed information
        /// about the payment gateways and charges to be displayed to the customer before payment processing.
        /// </summary>
        /// <param name="payment">Initiate object for payment integration</param>
        /// <returns>Api Response</returns>
        [HttpPost("InitiatePayment")]
        public async Task<IActionResult> InitiatePaymentAsync([FromBody] InitiatePaymentRequest payment)
        {            
            var response = await _paymentService.InitiatePaymentAsync(payment);

            if (response.IsSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Data, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.ValidationErrors, DbReturnValue.CreationFailed));
            }
        }

        /// <summary>
        /// Direct Payment
        /// </summary>
        /// <param name="payment">Payment object</param>
        /// <param name="invoiceKey">Invoice Key param</param>
        /// <param name="paymentGatewayId">Payment Gateway Id</param>
        /// <returns>Api Response</returns>
        [HttpPost("DirectPayment")]
        public async Task<IActionResult> DirectPaymentAsync([FromBody] DirectPaymentRequest payment, string invoiceKey, int paymentGatewayId,int? userId)
        {            
            var response = await _paymentService.DirectPaymentAsync(payment, invoiceKey, paymentGatewayId);
            if (response.IsSuccess)
            {
                PaymentReference createReference = new PaymentReference();
                createReference.ExternalId = Convert.ToString(response.Data.PaymentId);
                createReference.UserId = userId;
                await _reference.AddAsync(createReference);
                return Ok(ApiResponse.OkResult(true, response.Data, DbReturnValue.CreateSuccess));                
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response, DbReturnValue.CreationFailed));
            }
        }
        #endregion
    }
}