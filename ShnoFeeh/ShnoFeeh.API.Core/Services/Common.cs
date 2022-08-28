using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Services
{
    public class Common : ICommon
    {
        private readonly IConfiguration _configuration;

        public Common(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUri(string uri)
        {
            var resourceUri = _configuration["URIs:" + uri + ""];
            return _configuration["URIs:BaseURI"] + resourceUri;
        }
        public string GetTapUri(string uri)
        {
            var resourceUri = _configuration["URIs:" + uri + ""];
            return _configuration["URIs:BaseTapURI"] + resourceUri;
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
    }
}
