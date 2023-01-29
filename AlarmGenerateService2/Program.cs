using Common;
using Common.RBAC;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService2
{
	class Program
	{
		static void Main(string[] args)
		{
            string srvCertCN = FormaterCer.ParseNameForCert(WindowsIdentity.GetCurrent().Name);
            string address = "net.tcp://localhost:9998/Service";
            string address2 = "net.tcp://localhost:9997/Replicator";

            NetTcpBinding binding = new NetTcpBinding();
            NetTcpBinding binding2 = new NetTcpBinding();

            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            binding2.Security.Mode = SecurityMode.Transport;
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding2.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host2 = new ServiceHost(typeof(Replicator));
            host2.AddServiceEndpoint(typeof(IReplicator), binding, address2);

            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService2), binding, address);

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
            host.Open();
            host2.Open();

            Console.WriteLine("Korisnik koji je pokrenuo servera :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Sekundarni servis je pokrenut.");

            
            Console.ReadLine();
        }
	}
}
