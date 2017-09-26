
using System.Web.Mvc;
using ImageSpriter.Processor;

namespace ImageSpriter.Web.Filters
{
    public class MyCustomAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            if (response.ContentType == "text/html")
            {
                response.Filter = new MyCustomStream(filterContext.HttpContext.Response.Filter);
            }
        }
    }
}