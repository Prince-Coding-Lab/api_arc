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
    public class ExceptionController : ControllerBase
    {
        #region Fields
        private readonly IExceptionService _exceptionService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public ExceptionController(IExceptionService exceptionService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _exceptionService = exceptionService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create Web exception logs
        /// </summary>
        /// <param name="ex">exception object</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExceptionLogCreateDto ex)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addException = _mapper.Map<ExceptionWeb>(ex);

            DatabaseResponse response = await _exceptionService.CreateWebException(addException);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));

        }
        /// <summary>
        /// Get all exceptions of api
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GetAllApiExceptions")]
        public async Task<IActionResult> GetApiExceptionsAsync()
        {
            DatabaseResponse response = await _exceptionService.GetAllApiExceptions();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));

            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        /// <summary>
        /// Get all exceptions of web
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GetAllWebExceptions")]
        public async Task<IActionResult> GetWebExceptionsAsync()
        {
            DatabaseResponse response = await _exceptionService.GetAllWebExceptions();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));

            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        #endregion
    }
}