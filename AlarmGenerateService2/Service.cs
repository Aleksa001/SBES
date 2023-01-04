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
				string message = $"Alarm:\n\tMessage:{a.Message}.\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}.";
				WriteInFile(message);
               
				//WriteInFile(a.Message);
			}
		}
		public static string fileName = "proba1.txt";
		public static string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

		public void WriteInFile(string message)
		{

			using (StreamWriter sw = new StreamWriter(path, true))
			{
				sw.WriteLine(message, true);
				sw.WriteLine("---------------------------------------------------------------------------");
			}


		}
	}
}
