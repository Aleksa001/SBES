using Common;
using Common.Logger;
using Common.RBAC;
using Common.Manager;
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


            Service s = new Service();
            s.NotReplicated();

            Console.WriteLine("Korisnik koji je pokrenuo servera :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Primarni servis je pokrenut.");
            Replicator primary = new Replicator();
            Console.ReadLine();


        }
    }
}
