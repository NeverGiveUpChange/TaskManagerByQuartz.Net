using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Web.CommonService
{
    public interface IServiceGetter
    {
        T GetByKeyed<T,K>(K serviceKey);
    }
}
