using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _env;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception Filter {FilerName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}"
                , nameof(HandleExceptionFilter)
                , nameof(OnException)
                , context.Exception.GetType().Name
                , context.Exception.Message);

            if (_env.IsDevelopment())
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };

        }
    }
}
