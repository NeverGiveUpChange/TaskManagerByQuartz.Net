
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RemoteServer
{
    class Program
    {
        static void Main(string[] args)

        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";//端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";//名称
            properties["quartz.scheduler.exporter.channelType"] = "tcp";//通道名
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";
            properties["quartz.jobStore.clustered"] = "true";//集群配置
            //下面为指定quartz持久化数据库的配置
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            properties["quartz.jobStore.dataSource"] = "myDS";
            properties["quartz.dataSource.myDS.connectionString"] = @"Data Source=.;Initial Catalog=QuartzManager;User ID=sa;Password=123456";
            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.ListenerManager.AddJobListener(new MyJobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.Start();

        }
    }
}
