using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters.ResouceFilters
{
    public class TokenResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
          context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
        }
    }
}
