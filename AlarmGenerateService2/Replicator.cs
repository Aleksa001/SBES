using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;
using Common.Manager;
using Common.RBAC;

namespace AlarmGenerateService2
{
	public class Replicator 
	{
		
		public Replicator()
		{
            string srvCertCN = FormaterCer.ParseNameForCert(WindowsIdentity.GetCurrent().Name);

            string address2 = "net.tcp://localhost:9997/Replicator";


            NetTcpBinding binding2 = new NetTcpBinding();

            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost host2 = new ServiceHost(typeof(ReplicatorService));
            host2.AddServiceEndpoint(typeof(IReplicator), binding2, address2);

            // koristi se chain trust 
            host2.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            host2.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            // podevanje svog sertifikata kojim se predstavlja
            host2.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host2.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host2.Description.Behaviors.Add(newAudit);


            host2.Open();

            Console.WriteLine("Korisnik koji je pokrenuo replikator :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Sekundarni replikator je pokrenut.");


            Console.ReadLine();
        }
	}
}
