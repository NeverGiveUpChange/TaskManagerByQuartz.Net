using Quartz;
using Quartz.Impl;
using Quartz.Net_Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Quartz.Net_Core.JobCommon
{
    public class SchedulerManager
    {
        static readonly object Locker = new object();
        static IScheduler _scheduler;
        public static Dictionary<string, IScheduler> ConnectionCache = new Dictionary<string, IScheduler>();
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

                foreach (var schedulerInstanceInfo in SchedulerData.schedulerInstanceIdEquivalentIp)
                {
                    try
                    {
                        GetScheduler(schedulerInstanceInfo.Key);
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

        public static IScheduler GetScheduler(string schedulerInstanceId)
        {
            IScheduler scheduler = null;
            var schedulerInstanceInfo = SchedulerData.schedulerInstanceIdEquivalentIp[schedulerInstanceId];
            if (schedulerInstanceInfo != null)
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = $"{channelType}://{schedulerInstanceInfo.Ip}:{schedulerInstanceInfo.Port}/{bindName}";
                var schedulerFactory = new StdSchedulerFactory(properties);
                scheduler = schedulerFactory.GetScheduler();
                ConnectionCache[schedulerInstanceId] = scheduler;
            }

            return scheduler;
        }
    }
}
