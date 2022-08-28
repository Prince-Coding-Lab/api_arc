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
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CampaginController : ControllerBase
    {

        #region Fields
        private readonly ICampaginService _campaginService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public CampaginController(ICampaginService campaginService,
            IMapper mapper, IConfiguration configuration)
        {
            _campaginService = campaginService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create a new Campaign
        /// </summary>
        /// <param name="campaign">New Campaign Object</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddCampaignDto campaign)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToCampaign = _mapper.Map<Campaign>(campaign);

            DatabaseResponse response = await _campaginService.AddCampaignAsync(addToCampaign);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(false, response.Results, DbReturnValue.RecordExists));
            }

        }
        /// <summary>
        /// Update a Campaign
        /// </summary>
        /// <param name="campaign">Updated object</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCampaignDto campaign)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateCampaign = _mapper.Map<Campaign>(campaign);

            DatabaseResponse response = await _campaginService.UpdateCampaignAsync(updateCampaign);

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
        /// Delete a Campaign
        /// </summary>
        /// <param name="campaignId">Unqiue ID</param>
        /// <returns>Api Response</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int campaignId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }

            DatabaseResponse response = await _campaginService.DeleteCampaignAsync(campaignId);

            if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.DeleteSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.ActiveTryDelete));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync(int? userId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _campaginService.GetCampaignsAsync(userId);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _campaginService.GetCampaignByIdAsync(id);

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