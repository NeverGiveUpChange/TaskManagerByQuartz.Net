using Quartz;
using Quartz.Impl;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;

namespace Quartz.Net_Core.JobCommon
{
    public class SchedulerManager
    {
        static readonly object Locker = new object();
        static IScheduler _scheduler;
        public  static  ConcurrentDictionary<string, IScheduler> ConnectionCache = new ConcurrentDictionary<string, IScheduler>();
        static readonly string channelType = ConfigurationManager.AppSettings["channelType"];
        static readonly string localIp = ConfigurationManager.AppSettings["localIp"];
        static readonly string port = ConfigurationManager.AppSettings["port"];
        static readonly string bindName = ConfigurationManager.AppSettings["bindName"];
        public static IScheduler Instance
        {
            get
            {
                if (_scheduler == null)
                {
                    lock (Locker)
                    {
                        if (_scheduler == null)
                        {
                            _scheduler = _getScheduler(localIp);
                        }
                    }
                }
                return _scheduler;
            }
        }
        private static IScheduler _getScheduler(string ip)
        {
            if (!ConnectionCache.ContainsKey(ip))
            {
                var properties = new NameValueCollection();

                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = $"{channelType}://{localIp}:{port}/{bindName}";
                var schedulerFactory = new StdSchedulerFactory(properties);
                _scheduler = schedulerFactory.GetScheduler();
                ConnectionCache[ip] = _scheduler;
            }
            return ConnectionCache[ip];
        }
    }
}
