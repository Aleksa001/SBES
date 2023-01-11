using AlarmGenerateService;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replicator
{
    class SecondReplicator : Replicator
    {
		public SecondReplicator(List<Alarm> a)
		{
			string srvCertCN = "ags2";
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			//binding.Security.Mode = SecurityMode.Transport;
			//binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			//binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9998/Service"), new X509CertificateEndpointIdentity(srvCert));

			using (ReplicatorProxy proxy = new ReplicatorProxy(binding, endpointAddress))
			{

				proxy.Receive(a);
			}
		}
		
			
    }
}
