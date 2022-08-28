using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Model;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController : ControllerBase
    {
        #region Fields
        private readonly IPaymentRefService _paymentRefService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public ReferenceController(IPaymentRefService paymentRefService,
            IMapper mapper, IConfiguration configuration)
        {
            _paymentRefService = paymentRefService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create a new payment reference
        /// </summary>
        /// <param name="reference">reference object</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddPaymentReferenceDto reference)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToReference = _mapper.Map<PaymentReference>(reference);

            DatabaseResponse response = await _paymentRefService.AddAsync(addToReference);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
        }

        /// <summary>
        /// Get all records for payment reference.
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            DatabaseResponse response = await _paymentRefService.GetAllAsync();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }

        /// <summary>
        /// Get by Id record of Payment Reference
        /// </summary>
        /// <param name="id">Id Param</param>
        /// <returns>Api Response</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            DatabaseResponse response = await _paymentRefService.GetByIdAsync(id);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        #endregion
    }
}