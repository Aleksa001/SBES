using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replicator
{
    class FirstReplicator : Replicator
    {
		public override void Receive(Alarm a)
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
