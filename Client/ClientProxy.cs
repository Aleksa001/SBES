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
        public ClientProxy(NetTcpBinding binding, string address): base(binding, address)
        {
            factory = this.CreateChannel();
        }
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



       


        public void  CreateNew(Alarm a)
        {
            try
            {
                factory.CreateNew(a);
           

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            
              
        }

        public void CurrentStateOfBase()
        {
            try
            {
                factory.CurrentStateOfBase();
			}
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Message);
            }
        }

        public void DeleteAll()
        {
            factory.DeleteAll();
        }

        public void DeleteForClient()
        {
            factory.DeleteForClient();
        }

        public void WriteInFile(Alarm a)
        {
            throw new NotImplementedException();
        }
    }
}
