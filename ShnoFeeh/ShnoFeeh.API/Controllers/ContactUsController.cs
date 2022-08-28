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
    public class ContactUsController : ControllerBase
    {
        #region Fields
        private readonly IContactUsService _contactUsService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors 
        public ContactUsController(IContactUsService contactUsService,
            IMapper mapper, IConfiguration configuration)
        {
            _contactUsService = contactUsService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Post contact us query and email to admin
        /// </summary>
        /// <param name="contactUs">Query object</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> PostAsync([FromBody] AddContactUsDto contactUs)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToContactUs = _mapper.Map<ContactUs>(contactUs);

            DatabaseResponse response = await _contactUsService.SendEmailAsync(addToContactUs);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }

        }
        /// <summary>
        /// Update the contact us information.
        /// </summary>
        /// <param name="contactus">updated contactus object</param>
        /// <returns>Api Response</returns>
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateContactUsDto contactus)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateContactUs = _mapper.Map<ContactUs>(contactus);

            DatabaseResponse response = await _contactUsService.UpdateEmailAsync(updateContactUs);

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
        /// Get all contactus queries
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GeteMails")]
        public async Task<IActionResult> GetEmailsAsync()
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _contactUsService.GetEmailsAsync();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }
        /// <summary>
        /// Get contact us query by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Api Response</returns>
        [HttpGet("GetEmailById")]
        public async Task<IActionResult> GetEmailByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _contactUsService.GetEmailByIdAsync(id);

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