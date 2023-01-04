﻿using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replicator
{
    class SecondReplicator : Replicator
    {
		public SecondReplicator(List<Alarm> a)
		{
			NetTcpBinding binding = new NetTcpBinding();

			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9998/Service"),
			 EndpointIdentity.CreateUpnIdentity("proba2"));

			using (ReplicatorProxy proxy = new ReplicatorProxy(binding, endpointAddress))
			{

				proxy.Receive(a);
			}
		}
		
			
    }
}
