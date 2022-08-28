using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private static ICommonService _commonService;
  
        public ExceptionMiddleware(RequestDelegate next, ICommonService commonService)
        {
            this.next = next;
            _commonService = commonService;
      
        }

        public async Task Invoke(HttpContext context)
        {
            string token = string.Empty;
            try
            {
                await next(context);

                //if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                //{
                //    await context.Response.WriteAsync("Woops! We 404'd");
                //}
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (ex is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (ex is MyException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;


            //CREATE EXCEPTION
            ExceptionDto exceptionDTO = new ExceptionDto();
            exceptionDTO.ExceptionMessage = ex.Message;
            exceptionDTO.ExceptionMethodName = "";
            exceptionDTO.ExceptionFileName = ex.TargetSite.Name;
            exceptionDTO.ExceptionSeverity = context.Response.StatusCode.ToString();
            exceptionDTO.ExceptionInnerException = ex.StackTrace;
            exceptionDTO.ExceptionType = ex.GetType().FullName;

            var response =  await _commonService.AddWebExceptionAsync(exceptionDTO);

            context.Response.Redirect("/Error");

            //return context.Redirect("/Error");

        }
    }
}
