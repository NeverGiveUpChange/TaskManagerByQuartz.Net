
using System;
using System.Net.Http;

namespace Quartz.Net_JobBase
{
    public  class JobBase : IJob
    {
        public  void Execute(IJobExecutionContext context)
        {
            try
            {
                HttpClient hc = new HttpClient();
                hc.GetAsync(context.JobDetail.JobDataMap["requestUrl"].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
