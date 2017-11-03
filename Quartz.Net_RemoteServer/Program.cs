
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
            //通道类型
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";
            //集群配置
            properties["quartz.jobStore.clustered"] = "true";
            //存储类型
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            //表名前缀
            properties["quartz.jobStore.tablePrefix"] = "qrtz_";
            //驱动类型
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";
            //数据源名称
            properties["quartz.jobStore.dataSource"] = "myDS";
            //连接字符串
            properties["quartz.dataSource.myDS.connectionString"]= @"";
            //版本
            properties["quartz.dataSource.myDS.provider"] = "MySql-65";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.ListenerManager.AddJobListener(new MyJobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.Start();

        }
    }
}
