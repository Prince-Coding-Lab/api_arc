using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IExceptionService
    {
        Task<DatabaseResponse> CreateWebException(ExceptionWeb ex);
        Task<DatabaseResponse> GetAllApiExceptions();
        Task<DatabaseResponse> GetAllWebExceptions();
    }
}
