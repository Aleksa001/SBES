using Common;
using Common.RBAC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace AlarmGenerateService
{
    public class Service : IService
    {
        public static List<Alarm> buffer = new List<Alarm>();

        public void CreateNew(Alarm a)
        {

            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;
            a.NameOfClient = Formater.ParseName(windowsIdentity.Name);
            Console.WriteLine($"Hello,{a.NameOfClient}");
            Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
            string message = $"Alarm:\n\tMessage:{a.Message}.\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}.";
            buffer.Add(a);
            Console.WriteLine(buffer.Count);
            WriteInFile(a);
        }

        public void CurrentStateOfBase()
        {
            //if (Thread.CurrentPrincipal.IsInRole("Reader"))
            //{
            Console.WriteLine("BAZA:");
            List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            foreach (string s in lst)
            {

                Console.WriteLine(s);
            }
            //}
            //else
            //{
            //   string name = Thread.CurrentPrincipal.Identity.Name;
            //   DateTime time = DateTime.Now;
            //   string message = String.Format("Access is denied. User {0} try to call Read method (time : {1}). " +
            //       "For this method need to be member of group Reader.", name, time.TimeOfDay);
            //   throw new FaultException<SecurityException>(new SecurityException(message));
            //}
        }

        public void DeleteAll()
        {
            File.Create(path).Close();
        }

        public void DeleteForClient()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            lst.RemoveAll(x => x.Split(':')[2].Equals(windowsIdentity.Name));
            File.WriteAllLines(path, lst);
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
