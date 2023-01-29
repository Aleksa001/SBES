using Common;
using Common.Manager;
using Common.RBAC;
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
	public class ReplicatorProxy : ChannelFactory<IReplicator>, IReplicator, IDisposable
	{
		public ReplicatorProxy(NetTcpBinding binding, string address) : base(binding, address)
		{
			factory = this.CreateChannel();
		}
		IReplicator factory;


		public ReplicatorProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
		{
            string cltCertCN = FormaterCer.ParseNameForCert(WindowsIdentity.GetCurrent().Name);
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


		public void WriteInFile(Alarm a)
		{
			throw new NotImplementedException();
		}
	}
}
