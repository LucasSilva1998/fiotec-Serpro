using fiotec_Serpro.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace fiotec_Serpro.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {     
        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                CpfInvalidoException => (int)HttpStatusCode.BadRequest,
                ServicoIndisponivelException => (int)HttpStatusCode.ServiceUnavailable,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                erro = exception.Message
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}