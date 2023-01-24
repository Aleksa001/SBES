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
       
        public static Alarm[] buffer2 = new Alarm[5];
        public static CustomAuthorizationManager princ = new CustomAuthorizationManager();
        public static int cnt = 0;
        public bool CreateNew(Alarm a)
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
                    //Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}");

                    buffer2[cnt] = a;
                    cnt++;
                  
                    Console.WriteLine(cnt.ToString() + "uspesno izgenerisan alarm");
                    //Console.WriteLine($"Duzina niza buffer2 je:{buffer2.Count()}\n");


                    WriteInFile(a);
                    return true;
                } else
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime time = DateTime.Now;
                    string message = String.Format("Access is denied. User {0} try to call AlarmGenerator method (time : {1}). " +
                        "For this method need to be member of group AlarmGenerator.", name, time.TimeOfDay);
                    throw new FaultException<SecurityException>(new SecurityException(message));
                    return false;
                }
            }
            catch (FaultException<SecurityException>)
            {

                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} try to call AlarmGenerator method (time : {1}). " +
                    "For this method need to be member of group AlarmGenerator.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
                return false;
            }

        }

        public List<string> CurrentStateOfBase()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formater.ParseName(principal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                Console.WriteLine("Read successfully executed");
                List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
                return lst;
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

        public bool DeleteAll()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            try
            {
                if (Thread.CurrentPrincipal.IsInRole("AlarmAdmin"))
                {
                
                        Console.WriteLine("Delete All successfully executed");
                        File.Create(path).Close();
                    return true;
                }
                else
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime time = DateTime.Now;
                    string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
                        "For this method need to be member of group Reader.", name, time.TimeOfDay);
                    throw new FaultException<SecurityException>(new SecurityException(message));
                    return false;
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

        public bool DeleteForClient()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

          
            try
            {
                 if (Thread.CurrentPrincipal.IsInRole("AlarmAdmin"))
                 {
                    IIdentity identity = Thread.CurrentPrincipal.Identity;
                    WindowsIdentity windowsIdentity = identity as WindowsIdentity;
                    string uName = Formater.ParseName(windowsIdentity.Name);
                    List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
                    while (lst.FindIndex(x => x.Contains(uName)) != -1)
                    {

                        int index = lst.FindIndex(x => x.Contains(uName));
                        lst.RemoveRange(index, 5);

                        File.WriteAllLines(path, lst);

                        Console.WriteLine("Delete for client successfully executed");
                    }
						return true;
                 }
                else
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime time = DateTime.Now;
                    string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
                        "For this method need to be member of group Reader.", name, time.TimeOfDay);
                    throw new FaultException<SecurityException>(new SecurityException(message));
                    return false;
                }

            }
            catch (Exception)
            {

              
            }
            return false;
        }


        public static string fileName = "proba.txt";
        public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        //public static StreamWriter sw = new StreamWriter(path,true);

        public void WriteInFile(Alarm a)
        {

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                
                sw.WriteLine("Ime klijenta:  " + a.NameOfClient , true);
                sw.WriteLine("Vreme generisanja alarma " + a.TimeOfGenerete.ToString(), true);
                sw.WriteLine("Poruka:  " + a.Message, true);
                sw.WriteLine("Rizik:  " + a.TypeOfRisk.ToString()+";", true);
                sw.WriteLine("------------------------------------", true);
            }


        }

       
    }
}
