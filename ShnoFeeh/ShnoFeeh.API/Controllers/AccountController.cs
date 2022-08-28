using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Infrastructure.Services;
using ShnoFeeh.API.Model;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors 
        public AccountController(IUserService userService,IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create a New User
        /// This is a anonymous action method.
        /// </summary>
        /// <param name="user">New User object</param>
        /// <returns>API Response</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addUser = _mapper.Map<AdminUsers>(user);
            if (!string.IsNullOrEmpty(addUser.Password))
            {
                addUser.Password = Cryptography.Encrypt(user.Password);
            }
            DatabaseResponse response = await _userService.CreateUserAsync(addUser);

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
        /// Update User information. 
        /// </summary>
        /// <param name="user">Updated user object</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UserUpdateDto user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateUser = _mapper.Map<AdminUsers>(user);

            DatabaseResponse response = await _userService.UpdateUserAsync(updateUser);

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
        /// Delete a user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }

            DatabaseResponse response = await _userService.DeleteUserAsync(userId);

            if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.DeleteSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Api Response</returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _userService.GetUserByIdAsync(userId);

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
        /// Get list of all users
        /// </summary>
        /// <param name="userId">This is user role Id which is optional</param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync(int? roleId)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _userService.GetUsersAsync(roleId);

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
        /// Login user with credential username password.
        /// </summary>
        /// <param name="model">a model hold username and password</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserAuthDto model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, Cryptography.Encrypt(model.Password));

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="user">It contains userid, current password and new password</param>
        /// <returns>Api Response</returns>
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]UserPasswordDto user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            else if (user.NewPassword != user.ConfirmPassword)
            {
                return Ok(ApiResponse.OkResult(true, null, DbReturnValue.PassNotMatch));
            }
            if (!string.IsNullOrEmpty(user.NewPassword) && !string.IsNullOrEmpty(user.CurrentPassword))
            {
                user.NewPassword = Cryptography.Encrypt(user.NewPassword);
                user.CurrentPassword = Cryptography.Encrypt(user.CurrentPassword);
            }
            DatabaseResponse response = await _userService.ChangePasswordAsync(user);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.PassNotMatch));
            }

        }

        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            DatabaseResponse response = await _userService.CheckValidUserAsync(email);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.EmailSent));
            }
            else if (response.ResponseCode == (int)DbReturnValue.EmailNotSent)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.EmailNotSent));

            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));

        }
        /// <summary>
        /// Reset Password of user
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            else if (resetPassword.Password != resetPassword.ConfirmPassword)
            {

                return Ok(ApiResponse.OkResult(false, null, DbReturnValue.PassNotMatch));
            }

            DatabaseResponse response = await _userService.ResetPasswordAsync(resetPassword);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else
            {

                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UsernameNotExist));
            }
        }
        /// <summary>
        /// Active email ID
        /// </summary>
        /// <param name="model">user object to verify token</param>
        /// <returns>Api Response</returns>
        [HttpPost("ActivateEmail")]
        public async Task<IActionResult> ActivateEmail([FromBody] VerificationTokenDto model)
        {// SEND EMAIL TO USER TO VERIFY HIS EMAIL, NORMALLY USED WHEN USER LOGIN IN WHILE EMAILCONFIRMED = FALSE

            if (!ModelState.IsValid)
            {
                new OperationResponse
                {
                    HasSucceeded = false,
                    IsDomainValidationErrors = true,
                    StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                    Message = string.Join("; ", ModelState.Values
                                             .SelectMany(x => x.Errors)
                                             .Select(x => x.ErrorMessage))
                };
            }

            var response = await _userService.ActivateEmail(model);

            if (response.ResponseCode == (int)DbReturnValue.EmailSent)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.EmailSent));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.EmailNotSent));
            }


        }
        /// <summary>
        /// Confirm token
        /// </summary>
        /// <param name="token">token value</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("ConfirmVerifytoken")]
        public async Task<IActionResult> ConfirmVerificationToken([FromBody] string token)
        {
            if (!ModelState.IsValid)
            {
                new OperationResponse
                {
                    HasSucceeded = false,
                    IsDomainValidationErrors = true,
                    StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                    Message = string.Join("; ", ModelState.Values
                                             .SelectMany(x => x.Errors)
                                             .Select(x => x.ErrorMessage))
                };
            }

            


            var response = await _userService.ConfirmVerificationToken(token);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.TokenExpired));
            }


        }
        
        [HttpPost("UploadProfilePhoto")]
        public async Task<UploadResponse> UploadFile(IFormFile file, string type)
        {
            string ext = string.Empty;

            // SETTING FILE NAME
            string ImageFileName = Guid.NewGuid().ToString();

            if (file == null)
                return new UploadResponse()
                {
                    HasSucceed = false,
                    FileName = null,
                    Message = "File is empty"

                };
            else if (!new CommonHelper().IsSupportedContentType_Images(file.ContentType))
            {
                return new UploadResponse()
                {
                    HasSucceed = false,
                    FileName = null,
                    Message = "unsupported file type"

                };
            }

            ext = file.FileName.Split(".")[1];

            ImageFileName = string.Concat(ImageFileName, ".", ext);

            AWSS3Config aWSS3Config = new AWSS3Config();
            aWSS3Config.AWSAccessKey = _iconfiguration.GetValue<string>("AwsS3:accessKey");
            aWSS3Config.AWSSecretKey = _iconfiguration.GetValue<string>("AwsS3:accessSecret");
            aWSS3Config.AWSBucketName = _iconfiguration.GetValue<string>("AwsS3:bucket_" + type);
            AmazonS3 amazonS3 = new AmazonS3(aWSS3Config);
            UploadResponse response = await amazonS3.UploadFile(file, _iconfiguration.GetValue<string>("AwsS3:subfolder_" + type) + "/" + ImageFileName);
            response.FileUrl = _iconfiguration.GetValue<string>("AwsS3:baseUrl") +  response.FileName;
            response.FileName = ImageFileName;
            return response;
        }

        #endregion
    }
}