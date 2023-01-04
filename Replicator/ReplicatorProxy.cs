using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;

namespace Replicator
{
	public class ReplicatorProxy : ChannelFactory<IService2>, IService2, IDisposable
	{
		public ReplicatorProxy(NetTcpBinding binding, string address) : base(binding, address)
		{
			factory = this.CreateChannel();
		}
		IService2 factory;


		public ReplicatorProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
		{
			factory = this.CreateChannel();
			//Credentials.Windows.AllowNtlm = false;
		}


		public void Receive(List<Alarm> a)
		{
			factory.Receive(a);
		}

		public void WriteInFile(string message)
		{
			throw new NotImplementedException();
		}
	}
}
