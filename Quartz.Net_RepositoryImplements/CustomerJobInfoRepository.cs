using Quartz.Net_EFModel;
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
        private readonly QuartzManagerEntities _dbContext;
        public CustomerJobInfoRepository()
        {
            _dbContext = DbContextFactory.DbContext;
        }
        public int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl)
        {
            var customerJobInfoModel = _dbContext.Customer_JobInfo.Add(new Customer_JobInfo
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

        public int UpdateCustomerJobInfo(Customer_JobInfo customerJobInfoModel)
        {

            _dbContext.SaveChanges();
            return customerJobInfoModel.Id;
        }

        public Tuple<IQueryable<Customer_JobInfo>, int> LoadCustomerInfoes<K>(Expression<Func<Customer_JobInfo,bool>>whereLambda, Expression<Func<Customer_JobInfo, K>> orderByLambda, bool isAsc,int pageIndex,int pageSize)
        {
            int totalCount = 0;
            var customerJobInfoModelQueryable = _dbContext.Customer_JobInfo.Where(whereLambda);

            totalCount = customerJobInfoModelQueryable.Count();
            return new Tuple<IQueryable<Customer_JobInfo>, int>(isAsc? customerJobInfoModelQueryable.OrderBy(orderByLambda).Skip(pageIndex-1).Take(pageSize): customerJobInfoModelQueryable.OrderByDescending(orderByLambda).Skip((pageIndex-1)*pageSize).Take(pageSize), totalCount);

        }
        public Customer_JobInfo LoadCustomerInfo(Expression<Func<Customer_JobInfo, bool>> whereLambda)
        {
            return _dbContext.Customer_JobInfo.SingleOrDefault(whereLambda);
        }
    }
}
