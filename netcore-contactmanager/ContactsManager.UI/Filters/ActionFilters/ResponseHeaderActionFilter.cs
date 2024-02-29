using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        private string Key { get; set; }
        private string Value { get; set; }
        private int Order { get; set; }

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }
   

        //Controller -> FilterFactory -> Filter
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order;

            return filter;
        }
    }

    //public class ResponseHeaderActionFilter : ActionFilterAttribute
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int Order { get; set; }

        //public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }

      /*
        public void OnActionExecuted(ActionExecutedContext context)
        {

            _logger.LogInformation("{FilterName}.{Action} method called"
                , typeof(ResponseHeaderActionFilter)
                , nameof(OnActionExecuted));

            context.HttpContext.Response.Headers.Add(Key, Value);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{Action} method called"
              , typeof(ResponseHeaderActionFilter)
              , nameof(OnActionExecuting));
        }
      */
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            #region OnActionExecuting
            _logger.LogInformation("{FilterName}.{Action} method called"
            , typeof(ResponseHeaderActionFilter)
            , nameof(OnActionExecutionAsync));
            #endregion

            await next();

            #region OnActionExecuted
            _logger.LogInformation("{FilterName}.{Action} method called"
             , typeof(ResponseHeaderActionFilter)
             , nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers.Add(Key??string.Empty, Value);
            #endregion

        }
    }
}
