using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Core.Services;
using ShnoFeeh.API.Model;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsPricesController : ControllerBase
    {
        #region Fields
        private readonly IAdsPricesService _adsPricesService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public AdsPricesController(IAdsPricesService adsPricesService,
            IMapper mapper, IConfiguration configuration)
        {
            _adsPricesService = adsPricesService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Get all ads prices list
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            DatabaseResponse response = await _adsPricesService.GetAdsPricesAsync();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }

        /// <summary>
        /// Filter ads price by Id
        /// </summary>
        /// <param name="adPriceId">Unique Id of ads price</param>
        /// <returns>Api Response</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int adPriceId)
        {
            DatabaseResponse response = await _adsPricesService.GetPriceByIdAsync(adPriceId);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }

        /// <summary>
        /// Update Ads Price
        /// </summary>
        /// <param name="price">Update Price</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAdsPricesDto price)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updatePrice = _mapper.Map<AdsPrices>(price);

            DatabaseResponse response = await _adsPricesService.UpdateAdsPriceAsync(updatePrice);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));

        }
        #endregion
    }
}