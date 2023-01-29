using AlarmGenerateService;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGenerateService2
{
	public class ReplicatorService : IReplicator
	{
		public void Receive(List<Alarm> alarmi)
		{
			Console.WriteLine("Podaci replicirani uspesno");
			foreach (var a in alarmi)
			{
				//Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
				//Console.WriteLine("\n-----------------------------------------------------------------------------------------\n");
				Service s = new Service();
				s.WriteInFile(a);
				Console.WriteLine("Uspesan upis");

			}
			
			
		}
	}
}
