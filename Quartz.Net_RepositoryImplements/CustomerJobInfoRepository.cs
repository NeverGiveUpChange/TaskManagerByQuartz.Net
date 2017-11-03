
using Quartz.Net_EFModel_MySql;
using Quartz.Net_RepositoryInterface;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;

namespace Quartz.Net_RepositoryImplements
{
    [Export("CustomerJobInfoRepository",typeof(ICustomerJobInfoRepository))]
    internal class CustomerJobInfoRepository : ICustomerJobInfoRepository
    {
        private readonly bihu_analyticsEntities _dbContext;
        public CustomerJobInfoRepository()
        {
            _dbContext = DbContextFactory.DbContext;
        }
        public int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl)
        {
            var customerJobInfoModel = _dbContext.customer_quartzjobinfo.Add(new customer_quartzjobinfo
            {
                CreateTime = DateTime.Now,
                Cron = cron,
                Description = jobDescription,
                JobGroupName = jobGroupName,
                JobName = jobName,
                TriggerState = -1,
                TriggerName = triggerName,
                TriggerGroupName = triggerGroupName,
                DLLName = "Quartz.Net_JobBase.dll",
                FullJobName = "Quartz.Net_JobBase.JobBase",
                 RequestUrl= requestUrl,
                Deleted = false
            });
            _dbContext.SaveChanges();
            return customerJobInfoModel.Id;
        }

        public int UpdateCustomerJobInfo(customer_quartzjobinfo customerJobInfoModel)
        {

            _dbContext.SaveChanges();
            return customerJobInfoModel.Id;
        }

        public Tuple<IQueryable<customer_quartzjobinfo>, int> LoadCustomerInfoes<K>(Expression<Func<customer_quartzjobinfo, bool>>whereLambda, Expression<Func<customer_quartzjobinfo, K>> orderByLambda, bool isAsc,int pageIndex,int pageSize)
        {
            int totalCount = 0;
            var customerJobInfoModelQueryable = _dbContext.customer_quartzjobinfo.Where(whereLambda);

            totalCount = customerJobInfoModelQueryable.Count();
            return new Tuple<IQueryable<customer_quartzjobinfo>, int>(isAsc? customerJobInfoModelQueryable.OrderBy(orderByLambda).Skip(pageIndex-1).Take(pageSize): customerJobInfoModelQueryable.OrderByDescending(orderByLambda).Skip((pageIndex-1)*pageSize).Take(pageSize), totalCount);

        }
        public customer_quartzjobinfo LoadCustomerInfo(Expression<Func<customer_quartzjobinfo, bool>> whereLambda)
        {
            return _dbContext.customer_quartzjobinfo.SingleOrDefault(whereLambda);
        }
    }
}
