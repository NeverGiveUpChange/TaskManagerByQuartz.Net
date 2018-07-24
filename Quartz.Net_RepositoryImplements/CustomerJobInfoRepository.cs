
using Quartz.Net_EFModel_MySql;
using Quartz.Net_Infrastructure.QueryableExtensionUtil;
using Quartz.Net_RepositoryInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Quartz.Net_RepositoryImplements
{
    
    internal class CustomerJobInfoRepository : ICustomerJobInfoRepository
    {
        private readonly bihu_analyticsEntities _dbContext;

        public CustomerJobInfoRepository(bihu_analyticsEntities _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl, int? cycle, int? repeatCount, string triggerType)
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
                DLLName = ConfigurationManager.AppSettings["dllName"],
                FullJobName = ConfigurationManager.AppSettings["FullJobName"],
                RequestUrl = requestUrl,
                Deleted = false,
                Cycle = cycle,
                TriggerType = triggerType,
                RepeatCount = repeatCount

            });
            _dbContext.SaveChanges();
            return customerJobInfoModel.Id;
        }

        public async Task<int> UpdateCustomerJobInfo(customer_quartzjobinfo customerJobInfoModel)
        {

            List<string> updateFieldNames = new List<string>();
            ReflectObjectFields(customerJobInfoModel, updateFieldNames);

            //_dbContext.Entry(customerJobInfoModel).State = System.Data.Entity.EntityState.Detached;
            _dbContext.customer_quartzjobinfo.Attach(customerJobInfoModel);
            foreach (var fieldName in updateFieldNames)
            {
                _dbContext.Entry(customerJobInfoModel).Property(fieldName).IsModified = true;
            }
            await _dbContext.SaveChangesAsync();
            return customerJobInfoModel.Id;
        }
        public int UpdateCustomerJobInfo(customer_quartzjobinfo customerJobInfoModel, params string[] paramsFieldNames)
        {
            _dbContext.customer_quartzjobinfo.Attach(customerJobInfoModel);
            foreach (var fieldName in paramsFieldNames)
            {
                _dbContext.Entry(customerJobInfoModel).Property(fieldName).IsModified = true;

            }
            _dbContext.SaveChanges();
            return customerJobInfoModel.Id;
        }

        public Tuple<IQueryable<customer_quartzjobinfo>, int> LoadCustomerInfoes<K>(Expression<Func<customer_quartzjobinfo, bool>> whereLambda, Expression<Func<customer_quartzjobinfo, K>> orderByLambda, bool isAsc, int pageIndex, int pageSize)
        {

            var customerJobInfoModelQueryable = _dbContext.customer_quartzjobinfo.Where(whereLambda).AsNoTracking();
            var totalCount = customerJobInfoModelQueryable.Count();
            return new Tuple<IQueryable<customer_quartzjobinfo>, int>(isAsc ? customerJobInfoModelQueryable.OrderBy(orderByLambda).Skip(pageIndex - 1).Take(pageSize) : customerJobInfoModelQueryable.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize), totalCount);

        }

        public Tuple<IQueryable<customer_quartzjobinfo>, int> LoadCustomerInfoes(Expression<Func<customer_quartzjobinfo, bool>> whereLambda, string ordering, int pageIndex, int pageSize)
        {

            var customerJobInfoModelQueryable = _dbContext.customer_quartzjobinfo.Where(whereLambda).AsNoTracking();
            var totalCount = customerJobInfoModelQueryable.Count();
            return new Tuple<IQueryable<customer_quartzjobinfo>, int>(customerJobInfoModelQueryable.Order(ordering).PageBy((pageIndex - 1) * pageSize, pageSize), totalCount);

        }
        public customer_quartzjobinfo LoadCustomerInfo(Expression<Func<customer_quartzjobinfo, bool>> whereLambda)
        {
            return _dbContext.customer_quartzjobinfo.Where(whereLambda).AsNoTracking().SingleOrDefault();
        }

        private void ReflectObjectFields<T>(T TModel, List<string> updateFieldNames)
        {
            var type = TModel.GetType();
            foreach (var item in type.GetProperties())
            {
                var name = item.Name;
                var value = item.GetValue(TModel, null);
                var defaultValue = _defaultForType(item.PropertyType);
                _getUpdateFieldNames(name, value, defaultValue, updateFieldNames);
                _setModelFieldsValue(TModel, item, value, defaultValue);
            }

        }
        private void _setModelFieldsValue<T>(T TModel, PropertyInfo propertyInfo, object value, object defaultValue)
        {
            if (value == null || value.Equals(defaultValue))
            {
                var typeArray = propertyInfo.PropertyType.GetGenericArguments();
                if (!typeArray.Any() && defaultValue == null)
                {
                    propertyInfo.SetValue(TModel, string.Empty);
                }
                if (typeArray.Any())
                {
                    propertyInfo.SetValue(TModel, _defaultForType(typeArray[0]));
                }
            }
        }

        private void _getUpdateFieldNames(string fieldName, object value, object defaultValue, List<string> updateFieldNames)
        {
            if (value != null && !value.Equals(defaultValue))
            {
                updateFieldNames.Add(fieldName);
            }
        }
        private object _defaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

    }
}
