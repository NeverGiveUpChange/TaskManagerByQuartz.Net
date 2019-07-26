
using JobManagerByQuartz.App_Start;
using Quartz.Net_Model;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JobManagerByQuartz
{
    public class MvcApplication : System.Web.HttpApplication
    {
     

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoFacConfig.BuiderIocContainer();
            //CustomError.LoadLog4Config();

            var db = new DbContext();
            db.Db.DbFirst.CreateClassFile(@"E:\TaskManagerByQuartz.Net\Quartz.Net_Model\Models1");


        }
    }
}
