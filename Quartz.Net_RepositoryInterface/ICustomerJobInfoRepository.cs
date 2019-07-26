
using Models;
using Quartz.Net_Model.ViewModels;
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
        /// <returns>添加后任务编号</returns>
        int AddCustomerJobInfo(custom_job_infoes addJobModel);

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
        (List<custom_job_infoes>, int) LoadPageCustomerInfoes(Expression<Func<custom_job_infoes, bool>> whereExpression, Expression<Func<custom_job_infoes, object>> orderByExpression, bool isAsc, int pageIndex, int pageSize);
        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns></returns>
        List<custom_job_infoes> LoadSchedulerInfoes(Expression<Func<custom_job_infoes, bool>> whereExpression);
        /// <summary>
        /// 加载单个任务
        /// </summary>
        /// <param name="id">任务主键编号</param>
        /// <returns>单个任务信息</returns>
        custom_job_infoes LoadCustomerInfo(int id);
    }
}
