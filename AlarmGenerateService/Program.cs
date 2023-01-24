using Common;
using Common.Logger;
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
            string address = "net.tcp://localhost:9999/Service";
           

            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService), binding, address);

            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();


            host.Open();
            

            Console.WriteLine("Korisnik koji je pokrenuo servera :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Primarni servis je pokrenut.");
            Console.WriteLine("Pokrecem komunicakiju sa Replikatorom.");

           // binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string srvCertCN = "replikator";
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            // podesavanje binding-a da podrzi autentifikaciju uz pomoc sertifikata
            
            

            EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9997/Replicator")/*, new X509CertificateEndpointIdentity(srvCert)*/);

		
            using(ServerProxy proxy = new ServerProxy(binding, endpointAddress))
            {
                //proxy.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                //proxy.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                while (true) { 
                    
                    if (Service.cnt == 5)
                    {
                        Console.WriteLine("BUFFER JE POPUNJEN I SPREMNO JE ZA REPLIKACIJU!!!");
                        try
                        {  
                            Service.cnt = 0;
                          //  Audit.ReplicationInitiated();
                            proxy.Receive(Service.buffer2.ToList());
                          
                            Console.WriteLine($"Trenutna vredonst CNT je {Service.cnt}\n");

                        }
                        catch (Exception e)
                        {
                           // Audit.ReplicationFailed();
                            Console.WriteLine(e);
                        }
                       
                      
                    }
                }
            }


            Console.ReadLine();


        }
    }
}
