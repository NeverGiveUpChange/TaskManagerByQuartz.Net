using Quartz.Net_Web.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Quartz.Net_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            #region 设置Mef在Mvc中初始化
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new App_Start.MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);
            #endregion

        }
    }
}
