
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Net_RemoteServer.Listeners;
using System.Collections.Specialized;
using System.Configuration;
using Topshelf;

namespace Quartz.Net_RemoteServer
{
    public class QuartzServer : ServiceControl
    {
        private ISchedulerFactory schedulerFactory;
        private IScheduler scheduler;
        private NameValueCollection properties = new NameValueCollection();

        public QuartzServer()
        {
            QuartzConfig();
            CreateSchedulerFactory();
            GetScheduler();
            AddJobListener();
        }
        protected void QuartzConfig()
        {
            properties["quartz.scheduler.instanceName"] = "RemoteServer";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["lazy-init"] = "false";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.scheduler.exporter.type"] = "Quartz.Net_RemoteServer.RemotingSchedulerExporterOverrided.RemotingSchedulerExporterOverrided, Quartz.Net_RemoteServer";
            properties["quartz.scheduler.exporter.port"] = ConfigurationManager.AppSettings["port"];
            properties["quartz.scheduler.exporter.bindName"] = ConfigurationManager.AppSettings["bindName"];//名称
            //通道类型
            properties["quartz.scheduler.exporter.channelType"] = ConfigurationManager.AppSettings["channelType"];
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "false";
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
            properties["quartz.dataSource.myDS.connectionString"] = ConfigurationManager.AppSettings["connectionString"];
            //版本
            properties["quartz.dataSource.myDS.provider"] = "MySql-695";
            properties["quartz.scheduler.instanceId"] = "AUTO";
          

        }
        protected void CreateSchedulerFactory()
        {

            schedulerFactory = new StdSchedulerFactory(properties);
        }
        protected void GetScheduler()
        {
            scheduler = schedulerFactory.GetScheduler();
        }
        protected void AddJobListener()
        {
            //scheduler.ListenerManager.AddTriggerListener(new MyTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
            scheduler.ListenerManager.AddJobListener(new MyJobListener(), GroupMatcher<JobKey>.AnyGroup());

            scheduler.ListenerManager.AddSchedulerListener(new MySchedulerListener());
        }
        public virtual void Start()
        {
            scheduler.Start();

        }
        public virtual void Stop()
        {
            scheduler.Shutdown(true);

        }
        public virtual void Pause()
        {
            scheduler.PauseAll();
        }

        public virtual void Resume()
        {
            scheduler.ResumeAll();
        }

        public bool Start(HostControl hostControl)
        {
            Start();
            return true;
        }
        public bool Stop(HostControl hostControl)
        {
            Stop();
            return true;
        }
        public bool Pause(HostControl hostControl)
        {

            Pause();
            return true;
        }
        public bool Continue(HostControl hostControl)
        {

            Resume();
            return true;
        }
    }
}
