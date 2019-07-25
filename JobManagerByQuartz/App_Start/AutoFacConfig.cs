using Autofac;
using Autofac.Integration.Mvc;

using JobManagerByQuartz.CommonService;
using Quartz.Net_Model;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace JobManagerByQuartz.App_Start
{
    public class AutoFacConfig
    {
        public static void BuiderIocContainer()
        {

            var builder = new ContainerBuilder();

            Assembly assembly = Assembly.Load("Quartz.Net_Core");
            assembly.GetTypes().Where(x => x.Name.EndsWith("Trigger") && !x.IsAbstract).Select(x => new { SelfType = x, BaseType = x.BaseType, Name = x.Name }).ToList().ForEach(x => builder.RegisterType(x.SelfType).Keyed(x.Name, x.BaseType).InstancePerLifetimeScope());
            builder.RegisterType<ServiceGetter>().As<IServiceGetter>().InstancePerLifetimeScope();
            builder.RegisterType<DbContext>().AsSelf().InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterAssemblyTypes(Assembly.Load("Quartz.Net_RepositoryImplements"))
            .Where(t => t.Name.EndsWith("Repository"))//查找所有程序集下面以Repository结尾的类  
            .AsImplementedInterfaces().InstancePerLifetimeScope(); //将找到的类和对应的接口放入IOC容器，并实现生命周期内唯一  
            //builder.RegisterType<bihu_analyticsEntities>().AsSelf().InstancePerRequest();
            var container = builder.Build(); //Build()方法是表示：创建一个容器  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//注册MVC容器  

        }
      
       
    }
}