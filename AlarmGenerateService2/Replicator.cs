using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;

namespace AlarmGenerateService2
{
	public class Replicator : IReplicator
	{
		
		public void Receive(List<Alarm> alarmi)
		{

			foreach (var a in alarmi)
			{
				//Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
				//Console.WriteLine("\n-----------------------------------------------------------------------------------------\n");
				Console.WriteLine("Podaci replicirani uspesno");

				
			}
		}
	}
}
