using Common;
using Common.RBAC;
using Nancy.Json;
using Newtonsoft.Json;
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


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Alarm> alarms = new List<Alarm>();
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

                    buffer2[cnt] = a;
                    cnt++;
                  
                    Console.WriteLine(cnt.ToString() + "\n Uspesno izgenerisan alarm");
                    WriteInFile(a);
                    return true;
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

        public bool DeleteForClient()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string uName = Formater.ParseName(principal.Identity.Name);
            Console.WriteLine("USER: "+uName);
            try
            {
                if (Thread.CurrentPrincipal.IsInRole("AlarmAdmin"))
                {
                    IIdentity identity = Thread.CurrentPrincipal.Identity;
                    WindowsIdentity windowsIdentity = identity as WindowsIdentity;
                    Console.WriteLine("BRISANJE!!! "+ windowsIdentity.Name);
                    alarms = ReadFromFile();

                    foreach (Alarm a in alarms)
                    {
                        Console.WriteLine("name fo client: "+a.NameOfClient);
                        if (a.NameOfClient == uName)
                        {
                            bool succ=alarms.Remove(a);
                            Console.WriteLine("BRISANJE IZ LISTE: "+succ.ToString());
                        }
                    }

                    if (alarms.Count() != 0)
                    {
                        foreach (Alarm a in alarms)
                        {
                            string json = serializer.Serialize(a);
                            File.AppendAllText(path, json + Environment.NewLine);    //gazi prethodni tekst u fajlu
                            Console.WriteLine("pisanjee");
                        }
                    } else
                    {
                        File.Create(path).Close();
                    }
                    

                    Console.WriteLine($"Delete for client {windowsIdentity.Name} successfully executed");
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
            catch
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
                    "For this method need to be member of group Reader.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            
        }


        public static string fileName = "proba.txt";
        public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

        public void WriteInFile(Alarm a)
        {
            string json = serializer.Serialize(a);
            File.AppendAllText(path, json+Environment.NewLine);
        }

        private List<Alarm> ReadFromFile()
        {
            try
            {
                string[] lst = File.ReadAllLines(path);

                foreach (string line in lst)
                {
                    Alarm a = serializer.Deserialize<Alarm>(line);
                    alarms.Add(a);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Read from file "+e.Message);
            }

            return alarms;
        }

        public void NotReplicated()
        {
            try
            {
                alarms=ReadFromFile();
                cnt = alarms.Count() % 5;
                if (cnt > 0)
                {
                    Console.WriteLine("Server je prethodno pukao, ostali su nereplicirani podaci.");
                    for (int i = 0; i < cnt; i++)
                    {
                        buffer2[i] = alarms[i];
                        Alarm a = alarms[i];
                        Console.WriteLine("\nIme klijenta: " + a.NameOfClient);
                        Console.WriteLine("\tVreme: " + a.TimeOfGenerete);
                        Console.WriteLine("\tPoruka: " + a.Message);
                        Console.WriteLine("\tRizik: " + a.TypeOfRisk.ToString());
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("tried deserialization... ", e.Message);
            }
        }
       
    }
}
