using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.BusinessService.Dto
{
  public  class ExceptionDto
    {
        public string ExceptionType { get; set; }
        public string ExceptionLineNumber { get; set; }
        public string ExceptionColumnNumber { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionFileName { get; set; }
        public string ExceptionMethodName { get; set; }
        public string ExceptionInnerException { get; set; }
        public string ExceptionSeverity { get; set; }
   
    }
}
