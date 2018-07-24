using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManagerByQuartz.CommonService
{
    public interface IServiceGetter
    {
        T GetByKeyed<T,K>(K serviceKey);
    }
}
