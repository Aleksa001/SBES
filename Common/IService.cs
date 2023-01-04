using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void CurrentStateOfBase();

        [OperationContract]
        void CreateNew(Alarm a);

        [OperationContract]
        void DeleteAll();

        [OperationContract]
        void DeleteForClient();

               

    }
}
