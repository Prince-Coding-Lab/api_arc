using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Enums;
using System.Linq;

namespace ShnoFeeh.API.Model
{
    public static class ApiResponse
    {
        public static OperationResponse ExceptionResponse()
        {
            return new OperationResponse
            {
                HasSucceeded = false,
                Message = StatusMessages.ServerError,
                StatusCode = ((int)ResponseStatus.ServerError).ToString(),
                IsDomainValidationErrors = false
            };
        }
        public static OperationResponse ValidationErrorResponse(ModelStateDictionary ModelState)
        {
            return new OperationResponse
            {
                HasSucceeded = false,
                IsDomainValidationErrors = true,
                StatusCode = ((int)ResponseStatus.BadRequest).ToString(),
                Message = string.Join("; ", ModelState.Values
                                                     .SelectMany(x => x.Errors)
                                                     .Select(x => x.ErrorMessage))
            };
        }
        public static OperationResponse OkResult(bool hasSucceeded, object result, DbReturnValue dbReturnValue)
        {
            return new OperationResponse
            {
                HasSucceeded = hasSucceeded,
                IsDomainValidationErrors = false,
                Message = EnumExtensions.GetDescription(dbReturnValue),
                ReturnedObject = result
            };
        }
    }
}
