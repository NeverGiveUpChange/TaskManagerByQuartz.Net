
using Quartz.Listener;
using Quartz.Net_EFModel;
using Quartz.Net_RepositoryInterface;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Quartz.Net_RemoteServer
{

    /// <summary>
    /// 监听类
    /// </summary>
    public class MyJobListener : IJobListener
    {

        [Import("CustomerJobInfoRepository")]
        public ICustomerJobInfoRepository _customerJobInfoRepository { get; set; }
        public MyJobListener()
        {
            Compose().ComposeParts(this);
        }
        public string Name
        {
            get
            {
                return "customerJobListener";
            }
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            int jobId = 0;
            string name = string.Empty;
            try
            {
                jobId = Convert.ToInt32(context.JobDetail.JobDataMap["jobId"]);
                name = context.Scheduler.GetTriggerState(context.Trigger.Key).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：{0}", ex.Message);

            }

            Console.WriteLine("任务编号{0}；执行时间：{1},状态：{2}", jobId, DateTime.Now,name);
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            int jobId = 0;
            string name = string.Empty;
            int triggerState = 0;
            Customer_JobInfo customerJobInfoModel = null;
            try
            {
                jobId = Convert.ToInt32(context.JobDetail.JobDataMap["jobId"]);
                name = context.Scheduler.GetTriggerState(context.Trigger.Key).ToString();
                triggerState = _changeType(context.Scheduler.GetTriggerState(context.Trigger.Key));
                customerJobInfoModel = _customerJobInfoRepository.LoadCustomerInfo(x => x.Id == jobId);
                customerJobInfoModel.TriggerState = triggerState;
                if (jobException == null)
                {


                    Console.WriteLine("任务编号{0}；执行时间：{1},状态：{2}", context.JobDetail.JobDataMap["jobId"], DateTime.Now, name);
                }
                else
                {
                    customerJobInfoModel.Exception = jobException.Message;
                    Console.WriteLine("jobId{0}执行失败：{1}", context.JobDetail.JobDataMap["jobId"], jobException.Message);
                }
                _customerJobInfoRepository.UpdateCustomerJobInfo(customerJobInfoModel);
            }
            catch (Exception ex) {
                Console.WriteLine("异常{0}", ex.Message);
            }
      
        }
        private CompositionContainer Compose()
        {
            AggregateCatalog aggregateCatalog = new AggregateCatalog();
            var thisAssembly = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            aggregateCatalog.Catalogs.Add(thisAssembly);
            var _container = new CompositionContainer(aggregateCatalog);

            return _container;
        }

        private int _changeType(TriggerState triggerState)
        {
            switch (triggerState)
            {
                case TriggerState.None: return -1;
                case TriggerState.Normal: return 0;
                case TriggerState.Paused: return 1;
                case TriggerState.Complete: return 2;
                case TriggerState.Error: return 3;
                case TriggerState.Blocked: return 4;
                default: return -1;
            }

        }
    }
}
