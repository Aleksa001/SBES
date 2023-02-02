using Common.Logger;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
	public class Replicator
	{
		public Replicator()
		{
            string srvCertCN = "ags2";
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            NetTcpBinding binding = new NetTcpBinding();
			
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			EndpointAddress endpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:9997/Replicator"), new X509CertificateEndpointIdentity(srvCert));

            Service s = new Service();

			using (ReplicatorProxy proxy = new ReplicatorProxy(binding, endpointAddress))
			{

                while (true)
                {
                    if (Service.cnt == 5)
                    {
                        Console.WriteLine("BUFFER JE POPUNJEN I SPREMNO JE ZA REPLIKACIJU!!!");
                       
                        try
                        {

                            Audit.ReplicationInitiated();
                            proxy.Receive(Service.buffer2.ToList());

                            List<Alarm> alarms = s.ReadFromFile();

                            foreach (Alarm a in alarms)
                            {
                                a.Replicated = true;
                            }
                            s.WriteReplicatedAlarms(alarms);
                            Service.cnt = 0;
                            Console.WriteLine($"Trenutna vredonst CNT je {Service.cnt}\n");

                        }
                        catch (Exception e)
                        {
                            Audit.ReplicationFailed();
                            Console.WriteLine(e);
                        }


                    }
                }
            }
		}
	}
}
