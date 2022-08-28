using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Infrastructure.Logging;
using ShnoFeeh.API.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAppLogger _appLogger;
        public ExceptionMiddleware(RequestDelegate next, IAppLogger appLogger)
        {
            this.next = next;
            this._appLogger = appLogger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            StackTrace st = new StackTrace(ex, true);
            //Get the first stack frame
            StackFrame frame = st.GetFrame(0);
            ExceptionLogDto exLog = new ExceptionLogDto
            {
                ExceptionLogId = new RandomSG().GetString(),
                ExceptionType = ex.GetType().FullName.ToString(),
                ExceptionInnerException = ex.InnerException == null ? "" : ex.InnerException.ToString(),
                ExceptionMessage = ex.Message,
                ExceptionSeverity = "ERROR",
                ExceptionFileName = frame.GetFileName(), //Get the file name
                ExceptionLineNumber = frame.GetFileLineNumber(),  //Get the line number
                ExceptionColumnNumber = frame.GetFileColumnNumber(), //Get the column number                      
                ExceptionMethodName = ex.TargetSite.ReflectedType.FullName // Get the method name

            };
            _appLogger.LogInformationAsync(exLog);

            var result = JsonConvert.SerializeObject(ApiResponse.ExceptionResponse());
            return context.Response.WriteAsync(result);

        }
    }
}
