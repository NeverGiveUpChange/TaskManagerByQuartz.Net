
using Quartz.Net_EFModel_MySql;
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
        /// <returns>添加后任务编号</returns>
        int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron,  string jobDescription, string requestUrl);
        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="whereLambda">新的任务模型</param>
        /// <returns>更新的任务编号</returns>
        int UpdateCustomerJobInfo(customer_quartzjobinfo customerJobInfoModel);
        /// <summary>
        /// 加载任务列表
        /// </summary>
        /// <typeparam name="K">排序表达式返回值类型</typeparam>
        /// <param name="whereLambda">条件表达式</param>
        /// <param name="orderByLambda">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页数量</param>
        /// <returns>查询数据集和总条数</returns>
        Tuple<IQueryable<customer_quartzjobinfo>, int> LoadCustomerInfoes<K>(Expression<Func<customer_quartzjobinfo, bool>> whereLambda, Expression<Func<customer_quartzjobinfo, K>> orderByLambda, bool isAsc,int pageIndex,int pageSize);
        /// <summary>
        /// 加载单个任务
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>单个任务</returns>
        customer_quartzjobinfo LoadCustomerInfo(Expression<Func<customer_quartzjobinfo, bool>> whereLambda);
    }
}
