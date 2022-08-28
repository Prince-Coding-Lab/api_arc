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
    public class CityController : ControllerBase
    {
        #region Fields
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors 
        public CityController(ICityService cityService, IMapper mapper, IConfiguration configuration)
        {
            _cityService = cityService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion
        #region Action Methods
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddCityDto city)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToCity = _mapper.Map<City>(city);

            DatabaseResponse response = await _cityService.AddCityAsync(addToCity);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCityDto city)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateCity = _mapper.Map<City>(city);

            DatabaseResponse response = await _cityService.UpdateCityAsync(updateCity);

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
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int cityId)
        {
            DatabaseResponse response = await _cityService.DeleteCityAsync(cityId);

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
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync(int countryId,bool isActive, string lang)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _cityService.GetCitiesAsync(countryId, isActive,lang);

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