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
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        #region Fields
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public CountryController(ICountryService countryService, IMapper mapper, IConfiguration configuration)
        {
            _countryService = countryService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Update country Active status
        /// </summary>
        /// <param name="country">Update country object</param>
        /// <returns>Api Response</returns>
        public async Task<IActionResult> PutAsync([FromBody] UpdateCountryDto country)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateCountry= _mapper.Map<Countries>(country);

            DatabaseResponse response = await _countryService.UpdateCountryAsync(updateCountry);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }

        /// <summary>
        /// Get all countries list active or inactive
        /// </summary>
        /// <param name="isActive">Countries filter by active or inactive</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync(bool isActive,string lang)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _countryService.GetCountriesAsync(isActive, lang);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));

            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }
        #endregion
    }
}