using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters.ResouceFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Filters.Any(item => item is SkipFilter))
                return;

            //TO DO: Logic to be executed before the action method is executed
            
        }
    }
}
