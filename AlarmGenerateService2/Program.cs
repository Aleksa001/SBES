﻿using Common;
using Common.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService2
{
	class Program
	{
		static void Main(string[] args)
		{
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/Service";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService2), binding, address);

            host.Open();


            Console.WriteLine("Korisnik koji je pokrenuo servera :" + Formater.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Servis je pokrenut.");

            Console.ReadLine();
        }
	}
}
