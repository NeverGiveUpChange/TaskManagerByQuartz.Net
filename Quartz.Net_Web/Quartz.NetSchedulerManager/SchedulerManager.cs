using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Quartz.Net_Web.Quartz.NetSchedulerManager
{
    public class SchedulerManager
    {
        static readonly object Locker = new object();
        static IScheduler _scheduler;
        static readonly ConcurrentDictionary<string, IScheduler> ConnectionCache = new ConcurrentDictionary<string, IScheduler>();
        const string channelType = "tcp";
        const string localIp = "127.0.0.1";
        const string port = "555";
        const string bindName = "QuartzScheduler";
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
                            _scheduler = GetScheduler(localIp);
                    }
                }
            }
                return _scheduler;
        }
    }
    public static IScheduler GetScheduler(string ip)
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