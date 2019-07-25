
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RepositoryInterface
{
    public interface ICustomerJobInfoRepository
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroupName">任务所在组名称</param>
        /// <param name="triggerName">触发器名称</param>
        /// <param name="triggerGroupName">触发器所在的组名称</param>
        /// <param name="cron">执行周期表达式</param>
        /// <param name="jobDescription">任务描述</param>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="cycle">重复周期</param>
        /// <param name="repeatCount">重复次数</param>
        /// <param name="triggerType">触发器类型</param>
        /// <returns>添加后任务编号</returns>
        int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl, int? cycle, int? repeatCount, string triggerType);

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="cloums">要更新的列</param>
        /// <param name="whereExpression">条件表达式</param>
        /// <returns>是否更新成功</returns>
        bool UpdateCustomerJobInfo(Expression<Func<custom_job_infoes, custom_job_infoes>> cloums, Expression<Func<custom_job_infoes, bool>> whereExpression);

        /// <summary>
        /// 加载任务列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页数量</param>
        /// <returns>查询数据集和总条数</returns>
        (List<custom_job_infoes>, int) LoadCustomerInfoes(Expression<Func<custom_job_infoes, bool>> whereExpression, Expression<Func<custom_job_infoes, object>> orderByExpression, bool isAsc, int pageIndex, int pageSize);
        /// <summary>
        /// 加载单个任务
        /// </summary>
        /// <param name="id">任务主键编号</param>
        /// <returns>单个任务信息</returns>
        custom_job_infoes LoadCustomerInfo(int id);
    }
}
