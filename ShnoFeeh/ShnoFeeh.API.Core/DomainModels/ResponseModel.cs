using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.DomainModels
{
    /// <summary>
    /// Response Model class with generic object
    /// </summary>
    public class ResponseModel<T>
    {
        #region Properties
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ValidationErrors> ValidationErrors { get; set; }
        public T Data { get; set; }
        #endregion
    }
    /// <summary>
    /// Response Model class.
    /// </summary>
    public class ResponseModel
    {
        #region Properties
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ValidationErrors> ValidationErrors { get; set; }
        public bool Data { get; set; }
        #endregion
    }
    /// <summary>
    /// Validation Errors class.
    /// </summary>
    public class ValidationErrors
    {
        #region Properties
        public string Name { get; set; }
        public string Error { get; set; }
        #endregion
    }
}
