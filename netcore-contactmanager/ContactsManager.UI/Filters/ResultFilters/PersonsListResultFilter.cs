using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context
            , ResultExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before"
                , nameof(PersonsListResultFilter)
                , nameof(OnResultExecutionAsync));

            context.HttpContext.Response.Headers.Add("Last-Modified", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            await next();

            _logger.LogInformation("{FilterName}.{MethodName} - after"
                , nameof(PersonsListResultFilter)
                , nameof(OnResultExecutionAsync));

            

        }
    }
}
