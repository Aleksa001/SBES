using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlarmGenerateService
{
    public class Service : IService
    {
        

        public void  CreateNew(Alarm a)
        {

            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;
            a.NameOfClient = windowsIdentity.Name;
            Console.WriteLine($"Hello,{windowsIdentity.Name}");
            Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
            string message = $"Alarm:\n\tMessage:{a.Message}.\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}.";
            WriteInFile(message);
        }

        public void CurrentStateOfBase()
        {
            List<string> lst = File.ReadAllLines(path).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            foreach(string s in lst)
            {
                Console.WriteLine(s);
            }
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
            lst.RemoveAll(x => x.Split('.')[1].Equals(windowsIdentity.Name));
            File.WriteAllLines(path, lst);
        }

      
        public static string fileName = "proba.txt";
        public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        //public static StreamWriter sw = new StreamWriter(path,true);
        
        public void WriteInFile(string message)
        {

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                    sw.WriteLine(message, true);
            }
                
            
        }

      
    }
}
