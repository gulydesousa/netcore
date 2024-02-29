using Serilog;

namespace CRUDexample.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public ExceptionHandlingMiddleware(RequestDelegate next
            , ILogger<ExceptionHandlingMiddleware> logger
            , IDiagnosticContext diagnosticContext)
        {
            //Representa el siguiente middleware en el pipeline
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}"
                        , ex.InnerException.GetType().ToString()
                        , ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}"
                      , ex.GetType().ToString()
                      , ex.Message);

                }
                //Para que se redireccione a nuestra página personalizada de error
                throw;
                //httpContext.Response.StatusCode = 500;
                //await httpContext.Response.WriteAsync("Error occured");
            }           
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
