using fiotec_Serpro.Domain.Exceptions;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace fiotec_Serpro.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log estruturado da exceção
                Log.Error(ex, "Erro ao processar requisição {Method} {Path} {QueryString}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString);

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
                status = context.Response.StatusCode,
                erro = exception.Message,
                timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
