
using Quartz.Net_Infrastructure.HttpClientUtil;

namespace Quartz.Net_Core.JobExcute
{
    internal class JobItem : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            HttpClientHelper _httpClient = new HttpClientHelper();
            var result = _httpClient.GetAsync(context.JobDetail.JobDataMap["requestUrl"].ToString());
        }
    }
}
