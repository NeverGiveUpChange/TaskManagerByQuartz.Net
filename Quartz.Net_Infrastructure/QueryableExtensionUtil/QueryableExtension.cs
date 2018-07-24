using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.QueryableExtensionUtil
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="skipCount">起始数量位置</param>
        /// <param name="takeCount">增量</param>
        /// <returns></returns>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int takeCount)
        {

            return query.Skip(skipCount).Take(takeCount);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="ordering">排序字段和类型</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<T> Order<T>(this IQueryable<T> query, string ordering, params object[] values)
        {
            return DynamicQueryable.OrderBy(query, ordering, values);
        }
    }
}
