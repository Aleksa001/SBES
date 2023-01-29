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
        List<string> CurrentStateOfBase();

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        bool CreateNew(Alarm a);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        bool DeleteAll();

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        bool DeleteForClient();

        [OperationContract]
        void WriteInFile(Alarm a);

    }
}
