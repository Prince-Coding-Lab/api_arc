using System;

namespace ShnoFeeh.API.Core.Dto
{
    /*
         This class contains 
         properties for ExceptionLog.
    */
    public class ExceptionLogDto
    {
        #region Properties
        public string ExceptionLogId { get; set; }
        public string ExceptionType { get; set; }
        public int ExceptionLineNumber { get; set; }
        public int ExceptionColumnNumber { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionFileName { get; set; }
        public string ExceptionMethodName { get; set; }
        public string ExceptionInnerException { get; set; }
        public string ExceptionSeverity { get; set; }
        #endregion
    }

    /*
         This class contains data transfer object
         properties for create ExceptionLog.
    */
    public class ExceptionLogCreateDto
    {
        #region Properties
        public string ExceptionType { get; set; }
        public string ExceptionInnerException { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionSeverity { get; set; }
        public string ExceptionFileName { get; set; }
        public string ExceptionLineNumber { get; set; }
        public string ExceptionColumnNumber { get; set; }
        public string ExceptionMethodName { get; set; }
        public DateTime CreatedDate { get; set; }
        #endregion
    }
}
