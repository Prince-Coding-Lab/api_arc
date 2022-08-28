using Microsoft.AspNetCore.Http;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Common.Methods
{
    public interface ICommonService
    {
        string GetUri(string uri);
        string GetWebUri(string uri);
        string GetUriWithoutBase(string uri);
        bool IsValidImage(out string message, IFormFile file);
        Task<ResponseDto<ExceptionDto>> AddWebExceptionAsync(ExceptionDto model);
    }
}
