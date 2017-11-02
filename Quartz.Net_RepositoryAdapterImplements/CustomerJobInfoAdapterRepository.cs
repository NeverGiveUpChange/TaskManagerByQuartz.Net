using Quartz.Net_RepositoryImplements;
using Quartz.Net_RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RepositoryAdapterImplements
{
    public class CustomerJobInfoAdapterRepository
    {
        public ICustomerJobInfoRepository _customerJobInfoRepository;
        public CustomerJobInfoAdapterRepository()
        {
            _customerJobInfoRepository = new CustomerJobInfoRepository();
        }
        public void AA(int jobId)
        {

            var customerJobInfoModel = _customerJobInfoRepository.LoadCustomerInfo(x => x.Id == jobId);
            customerJobInfoModel.TriggerState = 1;
            _customerJobInfoRepository.UpdateCustomerJobInfo(customerJobInfoModel);
        }
    }
}
