
using Quartz.Net_Infrastructure.QueryableExtensionUtil;
using Quartz.Net_Model;
using Quartz.Net_RepositoryInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using SqlSugar;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Models;

namespace Quartz.Net_RepositoryImplements
{
    
    internal class CustomerJobInfoRepository : ICustomerJobInfoRepository
    {
        private readonly DbContext _dbContext;

        public CustomerJobInfoRepository(DbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public int AddCustomerJobInfo(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron, string jobDescription, string requestUrl, int? cycle, int? repeatCount, string triggerType)
        {
            return _dbContext.customer_quartzjobinfoDb.InsertReturnIdentity(new custom_job_infoes
            {
                CreateTime = DateTime.Now,
                Cron = cron,
                Description = jobDescription,
                JobGroupName = jobGroupName,
                JobName = jobName,
                TriggerState = -1,
                TriggerName = triggerName,
                TriggerGroupName = triggerGroupName,
                DllName = ConfigurationManager.AppSettings["dllName"],
                FullJobName = ConfigurationManager.AppSettings["FullJobName"],
                RequestUrl = requestUrl,
                Deleted = 1,
                Cycle = cycle,
                TriggerType = triggerType,
                RepeatCount = repeatCount

            });
          
            
        }

        public bool UpdateCustomerJobInfo(Expression<Func<custom_job_infoes, custom_job_infoes>> cloums, Expression<Func<custom_job_infoes, bool>> whereExpression)
        {
            return _dbContext.customer_quartzjobinfoDb.Update(cloums, whereExpression);
        }


        public (List<custom_job_infoes>, int) LoadCustomerInfoes(Expression<Func<custom_job_infoes, bool>> whereExpression, Expression<Func<custom_job_infoes, object>> orderByExpression, bool isAsc, int pageIndex, int pageSize)
        {
            var pageModel = new PageModel() { PageIndex = pageIndex, PageSize = pageSize };
            var result = _dbContext.customer_quartzjobinfoDb.GetPageList(whereExpression, pageModel, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc);
            return (result, pageModel.PageCount);

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


        public custom_job_infoes LoadCustomerInfo(int id)
        {
            return _dbContext.customer_quartzjobinfoDb.GetById(id);
        }
    }
}
