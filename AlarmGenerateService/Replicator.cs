using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
	public class Replicator
	{
		public Replicator()
		{
			NetTcpBinding binding = new NetTcpBinding();
			
			
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

			EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9997/Replicator"));

			using (ReplicatorProxy proxy = new ReplicatorProxy(binding, endpointAddress))
			{

                while (true)
                {
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
		}
	}
}
