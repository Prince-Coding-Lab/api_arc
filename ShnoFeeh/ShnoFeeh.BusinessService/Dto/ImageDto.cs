using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.BusinessService.Dto
{
    public class Image
    {
        public IFormFile File { get; set; }
    }
    public class Images
    {
        public List<IFormFile> File { get; set; }
    }

    public class UploadImageResponse
    {
        [JsonProperty("hasSucceed")]
        public bool HasSucceed { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("fileUrl")]
        public string FileUrl { get; set; }
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
    }
}
