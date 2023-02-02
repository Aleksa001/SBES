using AlarmGenerateService;
using Common;
using Common.Logger;
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
			try
			{
				Console.WriteLine("Podaci replicirani uspesno");
				foreach (var a in alarmi)
				{
					Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
					Console.WriteLine("\n-----------------------------------------------------------------------------------------\n");
					Service s = new Service();
					s.WriteInFile(a);
					Console.WriteLine("Uspesan upis");
				}
				Audit.ReplicationSuccess();
			} catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
			
		}
	}
}
