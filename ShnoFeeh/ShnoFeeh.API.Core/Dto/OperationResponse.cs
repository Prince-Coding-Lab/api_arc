using ShnoFeeh.API.Core.Enums;

namespace ShnoFeeh.API.Core.Dto
{
    public class OperationResponse
    {
        #region Properties
        public bool HasSucceeded { get; set; }
        public bool IsDomainValidationErrors { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
        /// <summary>
        /// Used to envelope created/updated object
        /// </summary>
        public object ReturnedObject { get; set; }
        #endregion
        #region Constructors 
        public OperationResponse()
        {
            HasSucceeded = true;
            Message = "";
            IsDomainValidationErrors = false;
            StatusCode = ((int)ResponseStatus.OK).ToString();
        }
        public OperationResponse(bool hasSucceeded = true, string message = "")
        {
            HasSucceeded = hasSucceeded;
            Message = message;
        }
        #endregion
        #region Public Methods
        public ServerResponse GenerateResponse()
        {
            return new ServerResponse(this.ReturnedObject, HasSucceeded, Message);
        }
        #endregion
    }
    /// <summary>
    /// Server response convey status of request  along with a result data collection. 
    /// </summary>
    public class ServerResponse
    {
        #region Properties
        public object Result { get; set; }
        public bool HasSucceeded { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
        #endregion
        #region Constructors 
        public ServerResponse()
        {
            StatusCode = ((int)ResponseStatus.OK).ToString();
        }
        public ServerResponse(object result, bool hasSucceeded, string msg)
        {
            StatusCode = ((int)ResponseStatus.OK).ToString();
            Result = result;
            HasSucceeded = hasSucceeded;
            Message = msg;
        }
        #endregion
    }
    public class DatabaseResponse
    {
        #region Properties
        public int ResponseCode { get; set; }
        public object Results { get; set; }
        #endregion
    }
}
