using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void CurrentStateOfBase();

        [OperationContract]
        Alarm CreateNew();

        [OperationContract]
        void DeleteAll();

        [OperationContract]
        void DeleteForClient();

        [OperationContract]
        void SendAlarm(Alarm a);
       

    }
}
