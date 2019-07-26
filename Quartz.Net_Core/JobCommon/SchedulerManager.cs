using Quartz;
using Quartz.Impl;
using Quartz.Net_Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Quartz.Net_Core.JobCommon
{
    public class SchedulerManager
    {
        static readonly object Locker = new object();
        static IScheduler _scheduler;
        public static ConcurrentDictionary<string, IScheduler> ConnectionCache = new ConcurrentDictionary<string, IScheduler>();
        static readonly string channelType = ConfigurationManager.AppSettings["channelType"];
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
                            _scheduler = _getScheduler();
                        }
                    }
                }
                return _scheduler;
            }
        }
        private static IScheduler _getScheduler()
        {
            foreach (var item in ConnectionCache)
            {
                try
                {
                    if (item.Value.IsStarted)
                    {
                        _scheduler = item.Value;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            if (_scheduler == null)
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.proxy"] = "true";
                foreach (var item in SchedulerData.schedulerIdEquivalentIp)
                {
                    try
                    {
                        var ipPort = item.Value;
                        properties["quartz.scheduler.proxy.address"] = $"{channelType}://{ipPort.Ip}:{ipPort.Port}/{bindName}";
                        var schedulerFactory = new StdSchedulerFactory(properties);
                        _scheduler = schedulerFactory.GetScheduler();
                        ConnectionCache[item.Key] = _scheduler;
                        break;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            return _scheduler;
        }
    }
}
