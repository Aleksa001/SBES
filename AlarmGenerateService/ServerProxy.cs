using Common;
using Common.RBAC;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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
			string cltCertCN = FormaterCer.ParseNameForCert(WindowsIdentity.GetCurrent().Name);
            //Console.WriteLine($"Klijent cer {cltCertCN}\n");
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            
			factory = this.CreateChannel();
			//Credentials.Windows.AllowNtlm = false;
		}

		public void Receive(List<Alarm> a)
		{
            try
            {
				factory.Receive(a);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
			
		}
	}
}
