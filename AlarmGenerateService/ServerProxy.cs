using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
	public class ServerProxy : ChannelFactory<IReplicator>, IReplicator, IDisposable
	{
		public ServerProxy(NetTcpBinding binding, string address) : base(binding, address)
		{
			factory = this.CreateChannel();
		}
		IReplicator factory;


		public ServerProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
		{
			factory = this.CreateChannel();
			//Credentials.Windows.AllowNtlm = false;
		}

		public void Receive(List<Alarm> a)
		{
			factory.Receive(a);
		}
	}
}
