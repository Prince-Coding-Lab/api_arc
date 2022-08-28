using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Dto;
using ShnoFeeh.BusinessService.Factories;
using ShnoFeeh.BusinessService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Common.Methods
{
    public class CommonService : ICommonService
    {
        private readonly IConfiguration _configuration;
        private readonly IShnoFeehHttpFactory _httpClientFactory;

        public CommonService(IConfiguration configuration, IShnoFeehHttpFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public string GetUri(string uri)
        {
            var resourceUri = _configuration["URIs:" + uri + ""];
            return _configuration["URIs:BaseURI"] + resourceUri;
        }
        public string GetWebUri(string uri)
        {
            var resourceUri = _configuration["URIs:" + uri + ""];
            return _configuration["URIs:WebBaseUri"] + resourceUri;
        }


        public string GetUriWithoutBase(string uri)
        {
            var resourceUri = _configuration["URIs:" + uri + ""];
            return resourceUri;
        }
        public bool IsValidImage(out string message, IFormFile file)
        {
            string[] allowedExtensions = new string[] { ".jpg", ".png", ".gif" };
            message = string.Empty;
            if (file == null)
            {
                message = "Please select a file to upload.";
                return false;
            }
            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                message = "Please upload image only. supported files are jpg/png/gif.";
                return false;
            }
            if (file.Length > 1 * 1024 * 2048)
            {
                message = "File size should not exceed 2 MB.";
                return false;
            }
            message = "Image uploaded successfully.";
            return true;
        }

        //Exception
        public async Task<ResponseDto<ExceptionDto>> AddWebExceptionAsync(ExceptionDto model)
        {
            var response = await _httpClientFactory.PostAsyncReturnsObject<ExceptionDto, ResponseDto<ExceptionDto>>(model, GetUri(ApiUris.WebException), null);
            return response;
        }
    }
}
