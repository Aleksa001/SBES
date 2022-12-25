using AlarmGenerateService;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IService>, IService, IDisposable
    {
        IService factory;

        public ClientProxy(NetTcpBinding binding, string address): base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }


        public void  SendAlarm(Alarm a)
        {
            try
            {
                factory.SendAlarm(a);
            }
            catch(Exception e)
            {
               
                Console.WriteLine($"Error:{0}", e.Message);
               
            }

        }
       


        public Alarm CreateNew()
        {
            throw new NotImplementedException();
        }

        public void CurrentStateOfBase()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteForClient()
        {
            throw new NotImplementedException();
        }

        
    }
}
