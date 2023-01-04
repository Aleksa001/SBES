using AlarmGenerateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	[ServiceContract]
	public interface IService2
	{
		[OperationContract]
		void Receive(List<Alarm> a);

		[OperationContract]
		void WriteInFile(string message);
	}
}
