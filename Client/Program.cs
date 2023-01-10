using AlarmGenerateService;
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
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "wcfservice";
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:9999/Service";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + Formater.ParseName(WindowsIdentity.GetCurrent().Name));
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("proba"));
              //  new X509CertificateEndpointIdentity(srvCert));

           
            
            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                while (true)
                {
                    int choice = 0;
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Prikaz trenutnog stanja u bazi. ");
                    Console.WriteLine("2. Kreiranje novog alarma. ");
                    Console.WriteLine("3. Brisanje svih postojecih alarma. ");
                    Console.WriteLine("4. Brisanje personalizovanih alarma. ");
                    try
                    {
                        choice = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    switch (choice)
                    {
                        case 1:
                            // code block
                            proxy.CurrentStateOfBase();
                            break;
                        case 2:
                            // code block
                            Alarm a = new Alarm();
                            a.TypeOfRisk = a.CalculateRisk();
                            if (a.TypeOfRisk.ToString().Equals("Low"))
                                a.Message = "Rizik nizak.";
                            else if (a.TypeOfRisk.ToString().Equals("Medium"))
                                a.Message = "Pripazite, rizik raste";
                            else
                                a.Message = "Veoma visok rizik";
                            proxy.CreateNew(a);

                            break;
                        case 3:
                            proxy.DeleteAll();
                            break;
                        case 4:
                            proxy.DeleteForClient();
                            break;

                    }

                }
            }

            Console.ReadLine();


        }
    }
}
