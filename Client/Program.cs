using AlarmGenerateService;
using Common;
using Common.RBAC;
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
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + Formater.ParseName(WindowsIdentity.GetCurrent().Name));
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address));

           
            
            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                while (true)
                {
                    int choice = 0;
                    Console.WriteLine("\nOptions:");
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
                            List<Alarm> lista = proxy.CurrentStateOfBase();
                            if (lista.Count() > 0)
                            {
                                foreach (Alarm s in lista)
                                {
                                    Console.WriteLine($"Alarm:\n\tMessage:{s.Message}\n\tClient:{s.NameOfClient}.\n\tDate:{s.TimeOfGenerete}");
                                }
                            } else
                            {
                                Console.WriteLine("Nema nijednog alarma.");
                            }
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
                            a.TimeOfGenerete = DateTime.Now;
							if (proxy.CreateNew(a))
							{
                                Console.WriteLine("Uspesno kreiran alarm.");
                                Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}");
                            }

                            break;
                        case 3:
							if (proxy.DeleteAll())
							{
                                Console.WriteLine("Uspesno ste izbrisali sve iz baze.");
							}
							else
							{
                                Console.WriteLine("For this method need to be member of group Reader.");
							}
                            break;
                        case 4:
							if (proxy.DeleteForClient())
							{
                                Console.WriteLine("Uspesno izbrisan podatak iz baze");
							}
							else
							{
                                Console.WriteLine("Neuspesno brisanje");
							}
                            break;

                    }

                }
            }
        }
    }
}
