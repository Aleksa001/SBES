using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;
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

            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


            binding2.Security.Mode = SecurityMode.Transport;
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding2.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host2 = new ServiceHost(typeof(ReplicatorService));
            host2.AddServiceEndpoint(typeof(IReplicator), binding2, address2);

            // koristi se chain trust 
            /* host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
             host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
             // podevanje svog sertifikata kojim se predstavlja
             host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            */
            /* ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
             newAudit.AuditLogLocation = AuditLogLocation.Application;
             newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

             host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
             host.Description.Behaviors.Add(newAudit);
            */

            host2.Open();

            Console.WriteLine("Korisnik koji je pokrenuo replikator :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Sekundarni replikator je pokrenut.");


            Console.ReadLine();
        }
	}
}
