using AlarmGenerateService;
using Common;
using Common.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + Formater.ParseName(WindowsIdentity.GetCurrent().Name));
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
               EndpointIdentity.CreateUpnIdentity("proba"));

            Alarm a = new Alarm();
            Alarm a2 = new Alarm();
            
            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {

                a.TimeOfGenerete = DateTime.Now;
                a.Message = "probica";
                proxy.CreateNew(a);
                a2.TimeOfGenerete = DateTime.Now;
                a2.Message = "probica2";
                proxy.CreateNew(a2);
                proxy.CurrentStateOfBase();
                
            }

            Console.ReadLine();


        }
    }
}
