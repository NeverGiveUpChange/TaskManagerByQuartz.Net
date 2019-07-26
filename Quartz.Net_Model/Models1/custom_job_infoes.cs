using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class custom_job_infoes
    {
           public custom_job_infoes(){


           }
           /// <summary>
           /// Desc:任务当前所在节点实例ip
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CurrentSchedulerHost {get;set;}

           /// <summary>
           /// Desc:任务命名空间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string FullJobName {get;set;}

           /// <summary>
           /// Desc:任务起始节点实列Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string OriginSchedulerInstanceId {get;set;}

           /// <summary>
           /// Desc:任务当前节点实例别名ip->name
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CurrentSchedulerHostName {get;set;}

           /// <summary>
           /// Desc:主键Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:任务上次执行时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? PreTime {get;set;}

           /// <summary>
           /// Desc:任务当前所在节点实例Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CurrentSchedulerInstanceId {get;set;}

           /// <summary>
           /// Desc:任务组名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JobGroupName {get;set;}

           /// <summary>
           /// Desc:重复次数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? RepeatCount {get;set;}

           /// <summary>
           /// Desc:运行周期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Cycle {get;set;}

           /// <summary>
           /// Desc:任务名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JobName {get;set;}

           /// <summary>
           /// Desc:任务执行的业务逻辑地址
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string RequestUrl {get;set;}

           /// <summary>
           /// Desc:是否删除
           /// Default:
           /// Nullable:False
           /// </summary>           
           public byte Deleted {get;set;}

           /// <summary>
           /// Desc:任务开始时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? JobStartTime {get;set;}

           /// <summary>
           /// Desc:触发器组名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TriggerGroupName {get;set;}

           /// <summary>
           /// Desc:任务描述
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Description {get;set;}

           /// <summary>
           /// Desc:任务下一次执行时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? NextTime {get;set;}

           /// <summary>
           /// Desc:触发器名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TriggerName {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:任务所在的程序集
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DllName {get;set;}

           /// <summary>
           /// Desc:任务起始节点实例ip
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string OriginSchedulerHost {get;set;}

           /// <summary>
           /// Desc:任务状态
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int TriggerState {get;set;}

           /// <summary>
           /// Desc:周期表达式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Cron {get;set;}

           /// <summary>
           /// Desc:任务结束时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? EndTime {get;set;}

           /// <summary>
           /// Desc:任务起始节点实例别名ip->name
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string OriginSchedulerHostName {get;set;}

           /// <summary>
           /// Desc:任务类型
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TriggerType {get;set;}

    }
}
