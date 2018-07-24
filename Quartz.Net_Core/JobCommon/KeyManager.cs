using Quartz;
using System.Collections.Generic;

namespace Quartz.Net_Core.JobCommon
{
    internal class KeyManager
    {
        public static JobKey CreateJobKey(string jobName, string jobGroupName)
        {
            return new JobKey(jobName, jobGroupName);

        }

        public static TriggerKey CreateTriggerKey(string triggerName, string triggerGroupName)
        {
            return new TriggerKey(triggerName, triggerGroupName);
        }

        public static  JobDataMap CreateJobDataMap<T>(string propertyName, T propertyValue)
        {
            return new JobDataMap(new Dictionary<string, T>() { { propertyName, propertyValue } });
        }
    }
}
