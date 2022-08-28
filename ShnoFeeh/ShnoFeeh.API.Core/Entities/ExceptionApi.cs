using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
     /*
     This class contains properties of database entity ExceptionApi
    */
    public class ExceptionApi
    {
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionInnerException { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionSeverity { get; set; }
        public string ExceptionFileName { get; set; }
        public string ExceptionLineNumber { get; set; }
        public string ExceptionColumnNumber { get; set; }
        public string ExceptionMethodName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
