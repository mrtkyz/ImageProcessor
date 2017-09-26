using System.Web.Mvc;
using System.Web.Routing;
using ImageSpriter.Web.Filters;

namespace ImageSpriter.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new MyCustomAttribute());
        }
    }
}
