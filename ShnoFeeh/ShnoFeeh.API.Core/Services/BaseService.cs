using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Services
{
    /// <summary>
    /// base class for bearer token and base url configuration.
    /// </summary>
    public abstract class BaseService
    {
        private readonly IConfiguration _configuration;
        public BaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string SecretKey
        {
            get
            {
                return $"Bearer {_configuration.GetSection("MyFatoorahKeys")["SecretApiKey"]}";
            }
        }

        public string BaseUrl
        {
            get
            {
                return _configuration.GetSection("MyFatoorah")["BaseUrl"];
            }
        }
    }
}
