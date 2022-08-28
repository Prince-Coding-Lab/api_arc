using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.BusinessService.Dto
{
    public class ResponseDto<T>
    {
        public string StatusCode { get; set; }
        public T ReturnedObject { get; set; }
        public bool HasSucceeded { get; set; }
        public bool IsDomainValidationErrors { get; set; }
        public string Message { get; set; }
    }
}
