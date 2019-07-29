
using JobManagerByQuartz.App_Start;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_Model;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Linq;
using System.Collections.Generic;
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
            Task.Run(() =>
            {

                for (int i = 0; i < SchedulerData.schedulerInstanceIdEquivalentIp.Count; i++)
                {
                    var schedulerInstanceId = SchedulerData.schedulerInstanceIdEquivalentIp.ElementAt(i).Key;
                    var scheduler = SchedulerManager.ConnectionCache[schedulerInstanceId];
                    if (scheduler != null)
                    {
                        if (scheduler.IsShutdown)
                        {
                            //发送邮件
                            //更新服务异常信息
                        }
                    }
                }

            });
            var db = new DbContext();
            db.Db.DbFirst.CreateClassFile(@"E:\TaskManagerByQuartz.Net\Quartz.Net_Model\Models1");


        }
    }
}
