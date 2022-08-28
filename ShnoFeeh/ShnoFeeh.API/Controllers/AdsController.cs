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
using System;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {

        #region Fields
        private readonly IAdsService _adsService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public AdsController(IAdsService adsService,IMapper mapper, IConfiguration configuration)
        {
            _adsService = adsService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddAdsDto ads)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToAds = _mapper.Map<Ads>(ads);

            DatabaseResponse response = await _adsService.AddAdsAsync(addToAds);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            return Ok(ApiResponse.OkResult(false, response.Results, DbReturnValue.RecordExists));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAdsDto ads)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateAds = _mapper.Map<Ads>(ads);

            DatabaseResponse response = await _adsService.UpdateAdsAsync(updateAds);

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
        /// <summary>
        /// Get Ads by ID
        /// </summary>
        /// <param name="adId">Ad ID</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int adId, string lang)
        {
            DatabaseResponse response = await _adsService.GetAdByIdAsync(adId, lang);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        /// <summary>
        /// Get all ads filter by countryid or categoryid
        /// </summary>
        /// <param name="countryId">country id</param>
        /// <param name="catId">category id</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync(int? cityId, int? catId,int? statusId, string lang)
        {
            DatabaseResponse response = await _adsService.GetAdsAsync(cityId, catId,statusId,lang);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int? campaginId, int? adId)
        {
            DatabaseResponse response = await _adsService.DeleteAdsAsync(adId, campaginId);

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

        [HttpPost("UploadAdsPhoto")]
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
            response.FileUrl = _iconfiguration.GetValue<string>("AwsS3:baseUrl") + response.FileName;
            response.FileName = ImageFileName;
            return response;
        }
        #endregion
    }
}