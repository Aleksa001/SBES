using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlarmGenerateService;
using Common;

namespace Replicator
{
	public class Replicator : IReplicator
	{
		public void Forward(Alarm a)
		{
			IIdentity identity = Thread.CurrentPrincipal.Identity;
			WindowsIdentity windowsIdentity = identity as WindowsIdentity;
			a.NameOfClient = windowsIdentity.Name;
			Console.WriteLine($"Hello,{windowsIdentity.Name}");
			Console.WriteLine($"Alarm:\n\tMessage:{a.Message}\n\tClient:{a.NameOfClient}\n\tDate:{a.TimeOfGenerete}");
			string message = $"Alarm:\n\tMessage:{a.Message}.\n\tClient:{a.NameOfClient}.\n\tDate:{a.TimeOfGenerete}.";
		}
	}
}
