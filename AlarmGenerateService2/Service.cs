using AlarmGenerateService;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService2
{
	public class Service : IService2
	{
		public void Receive(List<Alarm> alarmi)
		{
			foreach (var a in alarmi)
			{
				Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
				Console.WriteLine("\n-----------------------------------------------------------------------------------------\n");
				string message = $"Alarm:\n\tMessage:{a.Message}.\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}\n\tRisk:{a.TypeOfRisk}.";
				WriteInFile(a);
               
				//WriteInFile(a.Message);
			}
		}
		public static string fileName = "proba1.txt";
		public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

		public void WriteInFile(Alarm a)
		{

			using (StreamWriter sw = new StreamWriter(path, true))
			{

				sw.WriteLine("Ime klijenta:  " + a.NameOfClient, true);
				sw.WriteLine("Vreme generisanja alarma " + a.TimeOfGenerete.ToString(), true);
				sw.WriteLine("Poruka:  " + a.Message, true);
				sw.WriteLine("Rizik:  " + a.TypeOfRisk.ToString() + ";", true);
				sw.WriteLine("------------------------------------", true);
			}


		}
	}
}
