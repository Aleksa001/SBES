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
		virtual public void Receive(Alarm a) { }
	}
}
