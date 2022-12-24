using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuanLyKhoaTu
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //Global.asax
        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;
            if (httpException == null) return;

            if (((System.Web.HttpException)httpException.InnerException).WebEventCode == System.Web.Management.WebEventCodes.RuntimeErrorPostTooLarge)
            {
                //handle the error
                Response.Write("Too big a file, dude"); //for example
                Response.Redirect("/Home/Error404");
            }
        }
    }
}
