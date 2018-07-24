using Quartz;
using Quartz.Listener;
using System;

namespace Quartz.Net_RemoteServer.Listeners
{
    internal class MyTriggerListener : TriggerListenerSupport
    {
        public override string Name
        {
            get
            {
                return "MyTriggerListener";
            }
        }

        /// <summary>
        /// Job完成时被调用
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="triggerInstructionCode"></param>
        public override void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        { 
            Console.WriteLine("任务调度完成");
        }
        /// <summary>
        /// Job执行时被调用
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        public override void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            Console.WriteLine("任务调度成功1");
        }
        /// <summary>
        /// 错过触发时间被调用
        /// </summary>
        /// <param name="trigger"></param>
        public override void TriggerMisfired(ITrigger trigger)
        {
            Console.WriteLine("任务调度错过");
        }
     
    }
}
