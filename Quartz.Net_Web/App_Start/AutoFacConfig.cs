using Autofac;
using Autofac.Integration.Mvc;
using JobManager_EfModel_MySql;
using JobManagerByQuartz.CommonService;
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

            Assembly assembly = Assembly.Load("JobManager_Core");
            assembly.GetTypes().Where(x => x.Name.EndsWith("Trigger") && !x.IsAbstract).Select(x => new { SelfType = x, BaseType = x.BaseType, Name = x.Name }).ToList().ForEach(x => builder.RegisterType(x.SelfType).Keyed(x.Name, x.BaseType).InstancePerLifetimeScope());
            builder.RegisterType<ServiceGetter>().As<IServiceGetter>().InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterAssemblyTypes(Assembly.Load("JobManager_RepositoryImplements"))
            .Where(t => t.Name.EndsWith("Repository"))//查找所有程序集下面以Repository结尾的类  
            .AsImplementedInterfaces().InstancePerLifetimeScope(); //将找到的类和对应的接口放入IOC容器，并实现生命周期内唯一  
            builder.RegisterType<bihu_analyticsEntities>().AsSelf().InstancePerRequest();
            var container = builder.Build(); //Build()方法是表示：创建一个容器  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//注册MVC容器  
            EfWarmUp();
        }
        /// <summary>
        /// Ef预热
        /// </summary>
        private static void EfWarmUp()
        {
            using (var _dbContext = new bihu_analyticsEntities())
            {
                //var bihu_analyticsEntities = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<bihu_analyticsEntities>();
                var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }

        }
    }
}