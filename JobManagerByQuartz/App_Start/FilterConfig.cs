using JobManagerByQuartz.CustomerFilters;
using System.Web;
using System.Web.Mvc;

namespace JobManagerByQuartz
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomerLogAtrribute());
        }
    }
}
