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
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IService>, IService, IDisposable
    {
        
        IService factory;


        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            //string cltCertCN = Formater.ParseName(WindowsIdentity.GetCurrent().Name);
            //this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            //this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            //this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
            //Credentials.Windows.AllowNtlm = false;
        }



       


        public bool  CreateNew(Alarm a)
        {
            try
            {
                return factory.CreateNew(a);
           

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmGenerator!");
                //Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmGenerator!");
                return false;
                //Console.WriteLine(e.Message);
            }
            
              
        }

        public List<string> CurrentStateOfBase()
        {
            List<string> lista = new List<string>();
            try
            {
              lista = factory.CurrentStateOfBase();
			}
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!");
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!");
                Console.WriteLine("Error while trying to Read : {0}", e.Message);
            }
            return lista;
        }

        public bool DeleteAll()
        {
            try
            {
              
               return factory.DeleteAll();
                

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmAdmin!");
                //Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmAdmin!");
                //Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool DeleteForClient()
        {
           
            try
            {
               return factory.DeleteForClient();
              

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmAdmin!");
                return false;
                //Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Korisnik nema pravo pristupa ovoj metodi!\tPotrebna permisija: AlarmAdmin!");
                return false;
                //Console.WriteLine(e.Message);
            }
        }

        public void WriteInFile(Alarm a)
        {
            throw new NotImplementedException();
        }
    }
}
