using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;
using Common.RBAC;
using Manager;

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
			string cltCertCN = Formater.ParseName(WindowsIdentity.GetCurrent().Name);
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
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
