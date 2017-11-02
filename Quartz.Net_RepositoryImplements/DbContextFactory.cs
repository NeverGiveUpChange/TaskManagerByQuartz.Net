using Quartz.Net_EFModel;
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
        public static QuartzManagerEntities DbContext
        {
            get
            {
                var _dbContext = CallContext.GetData("DbContext") as QuartzManagerEntities;
                if (_dbContext == null)
                {
                    _dbContext = new QuartzManagerEntities();
                    CallContext.SetData("DbContext", _dbContext);
                }
                return _dbContext;
            }

        }
    }
}
