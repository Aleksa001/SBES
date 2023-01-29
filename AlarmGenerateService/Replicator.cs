using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
	public class Replicator
	{
		public Replicator(List<Alarm> a)
		{
			NetTcpBinding binding = new NetTcpBinding();
			
			
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9997/Replicator"));

			using (ReplicatorProxy proxy = new ReplicatorProxy(binding, endpointAddress))
			{

				try
				{

					proxy.Receive(a);
					
				}
				catch (Exception e)
				{
					
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
