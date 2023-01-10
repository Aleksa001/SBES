using Common;
using Common.RBAC;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formater.ParseName(WindowsIdentity.GetCurrent().Name);
            string address = "net.tcp://localhost:9999/Service";

            NetTcpBinding binding = new NetTcpBinding();

            // podesavanje binding-a da podrzi autentifikaciju uz pomoc sertifikata
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            
            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService), binding, address);

            // koristi se chain trust 
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            // podevanje svog sertifikata kojim se predstavlja
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();


            host.Open();
            

            Console.WriteLine("Korisnik koji je pokrenuo servera :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Servis je pokrenut.");



           EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9997/Replicator"),
              EndpointIdentity.CreateUpnIdentity("proba2"));

		
            using(ServerProxy proxy = new ServerProxy(binding, endpointAddress))
            {
				while (true) { 
                    
                    if (Service.cnt == 5)
                    {
                       
                        proxy.Receive(Service.buffer2.ToList());
                        Service.cnt = 0;
                      
                    }
                }
            }


            Console.ReadLine();


        }
    }
}
