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
		public override void Receive(List<Alarm> alarmi)
		{

			Replicator.dataForRepl = alarmi;
			SecondReplicator secondReplicator = new SecondReplicator(alarmi);
			

		}
	}
}
