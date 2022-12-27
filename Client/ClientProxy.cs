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



       


        public void  CreateNew(Alarm a)
        {
            try
            {
                factory.CreateNew(a);
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error:{0}", e.Message);

            }
        }

        public void CurrentStateOfBase()
        {
            factory.CurrentStateOfBase();
        }

        public void DeleteAll()
        {
            factory.DeleteAll();
        }

        public void DeleteForClient()
        {
            factory.DeleteForClient();
        }

        
    }
}
