using Exceptions;
using Serilog;

namespace StockMarketSolution.Middleware
{
    public class ExceptionHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Exception> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<Exception> logger, IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FinnhubException ex)
            {
                LogException(ex);

                //context.Response.StatusCode = 500;
                //await context.Response.WriteAsync(ex.Message);

                throw;
            }
            catch (Exception ex)
            {
                LogException(ex);

                //context.Response.StatusCode = 500;
                //await context.Response.WriteAsync(ex.Message);

                throw;
            }
        }



        private void LogException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.InnerException.GetType().ToString(), ex.InnerException.InnerException.Message);

                    _diagnosticContext.Set("Exception", $"{ex.InnerException.InnerException.GetType().ToString()}, {ex.InnerException.InnerException.Message}, {ex.InnerException.InnerException.StackTrace}");
                }
                else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);

                    _diagnosticContext.Set("Exception", $"{ex.InnerException.GetType().ToString()}, {ex.InnerException.Message}, {ex.InnerException.StackTrace}");
                }
            }
            else
            {
                _logger.LogError("{ExceptionType} {ExceptionMessage}", ex.GetType().ToString(), ex.Message);

                _diagnosticContext.Set("Exception", $"{ex.GetType().ToString()}, {ex.Message}, {ex.StackTrace}");
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}