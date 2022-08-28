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
using ShnoFeeh.API.Model;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentsController : ControllerBase
    {
        #region Fields
        private readonly IAdvertismentService _advertismentService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public AdvertismentsController(IAdvertismentService advertismentService,
            IMapper mapper, IConfiguration configuration)
        {
            _advertismentService = advertismentService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create new advertisment.
        /// </summary>
        /// <param name="advertisment">advertisment object</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddAdvertismentDto advertisment)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToAdvertisment = _mapper.Map<Advertisments>(advertisment);

            DatabaseResponse response = await _advertismentService.AddAdvertismentAsync(addToAdvertisment);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.CategoryNotExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CategoryNotExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
        }
        /// <summary>
        /// Update advertisment detail
        /// </summary>
        /// <param name="advertisment">Updated advertisment object</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAdvertismentDto advertisment)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateToAdvertisment = _mapper.Map<Advertisments>(advertisment);

            DatabaseResponse response = await _advertismentService.UpdateAdvertismentAsync(updateToAdvertisment);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            else if (response.ResponseCode == (int)DbReturnValue.CategoryNotExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CategoryNotExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));

        }
        /// <summary>
        /// Get all advertisment filter by categoryid
        /// </summary>
        /// <param name="catId">category id</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync(int? categoryId, int? cityId)
        {
            DatabaseResponse response = await _advertismentService.GetAdvertismentsAsync(categoryId, cityId);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));

        }
        /// <summary>
        /// Delete a advertisment
        /// </summary>
        /// <param name="advertismentId">Unique Id of Advertisment</param>
        /// <returns>Api Response</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int advertismentId)
        {
            DatabaseResponse response = await _advertismentService.DeleteAdvertismentAsync(advertismentId);

            if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.DeleteSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.ActiveTryDelete));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        #endregion
    }
}