using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Common;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Infrastructure.Services;

namespace ShnoFeeh.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        IConfiguration _iconfiguration;
        public MasterController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }
        [HttpPost("UploadWebsitePhoto")]
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
    }
}
