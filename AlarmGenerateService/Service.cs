using Common;
using Common.RBAC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace AlarmGenerateService
{
    public class Service : IService
    {
        public static List<Alarm> buffer = new List<Alarm>();
        public static Alarm[] buffer2 = new Alarm[5];
        public static CustomAuthorizationManager princ = new CustomAuthorizationManager();
        public static int cnt = 0;
        public void CreateNew(Alarm a)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formater.ParseName(principal.Identity.Name);

            try
            {
                if (Thread.CurrentPrincipal.IsInRole("AlarmGenerator"))
                {

                    IIdentity identity = Thread.CurrentPrincipal.Identity;
                    WindowsIdentity windowsIdentity = identity as WindowsIdentity;
                    a.NameOfClient = Formater.ParseName(windowsIdentity.Name);
                    Console.WriteLine($"Hello,{a.NameOfClient}");
                    Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}");

                    buffer.Add(a);
                    buffer2[cnt] = a;
                    cnt++;
                    
                    Console.WriteLine(cnt.ToString());
                    WriteInFile(a);
                } else
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime time = DateTime.Now;
                    string message = String.Format("Access is denied. User {0} try to call AlarmGenerator method (time : {1}). " +
                        "For this method need to be member of group AlarmGenerator.", name, time.TimeOfDay);
                    throw new FaultException<SecurityException>(new SecurityException(message));
                }
            }
            catch (FaultException<SecurityException>)
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} try to call AlarmGenerator method (time : {1}). " +
                    "For this method need to be member of group AlarmGenerator.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }

        }

        public void CurrentStateOfBase()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formater.ParseName(principal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                Console.WriteLine("Read successfully executed");
            Console.WriteLine("BASE:");
            List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            foreach (string s in lst)
            {

                Console.WriteLine(s);
            }
            }
            else
            {
               string name = Thread.CurrentPrincipal.Identity.Name;
               DateTime time = DateTime.Now;
               string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
                   "For this method need to be member of group Reader.", name, time.TimeOfDay);
               throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }

        public void DeleteAll()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formater.ParseName(principal.Identity.Name);

            try
            {
                if (Thread.CurrentPrincipal.IsInRole("AlarmAdmin"))
                {
                
                        Console.WriteLine("Delete All successfully executed");
                        File.Create(path).Close();
                } 

            }
            catch (Exception)
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
                    "For this method need to be member of group Reader.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }

            
        }

        public void DeleteForClient()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formater.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("AlarmAdmin"))
            {
                IIdentity identity = Thread.CurrentPrincipal.Identity;
                WindowsIdentity windowsIdentity = identity as WindowsIdentity;

                List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
                lst.RemoveAll(x => x.Split(':')[3].Split('.')[0].Equals(windowsIdentity.Name));
                File.WriteAllLines(path, lst);

                Console.WriteLine("Delete for client successfully executed");
            }
        }


        public static string fileName = "proba.txt";
        public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        //public static StreamWriter sw = new StreamWriter(path,true);

        public void WriteInFile(Alarm a)
        {

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("Vreme generisanja alarma " + a.TimeOfGenerete.ToString(), true);
                sw.WriteLine("Ime klijenta  " + a.NameOfClient, true);
                sw.WriteLine("Poruka  " + a.Message, true);
                sw.WriteLine("Rizik  " + a.TypeOfRisk.ToString(), true);
                sw.WriteLine("------------------------------------", true);
            }


        }

       
    }
}
