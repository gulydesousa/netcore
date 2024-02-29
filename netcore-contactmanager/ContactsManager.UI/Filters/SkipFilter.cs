using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDexample.Filters
{
    public class SkipFilter: Attribute, IFilterMetadata
    {
    }
}
