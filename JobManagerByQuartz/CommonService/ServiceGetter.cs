using Autofac.Integration.Mvc;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Features.Indexed;

namespace JobManagerByQuartz.CommonService
{
    internal class ServiceGetter : IServiceGetter
    {
        public T GetByKeyed<T, K>(K serviceKey)
        {
            var IIndex = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<IIndex<K, T>>();
            //var IIndex = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IIndex<K, T>>();

            return IIndex[serviceKey];
        }
    }
}