
using Quartz.Net_EFModel_MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RepositoryImplements
{
    internal class DbContextFactory
    {
        public static bihu_analyticsEntities DbContext
        {
            get
            {
                var _dbContext = CallContext.GetData("DbContext") as bihu_analyticsEntities;
                if (_dbContext == null)
                {
                    _dbContext = new bihu_analyticsEntities();
                    CallContext.SetData("DbContext", _dbContext);
                }
                return _dbContext;
            }

        }
    }
}
