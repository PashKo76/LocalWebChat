using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebChat
{
	internal interface INetworkHandler
	{
		void Send(string info);
		void Stop();
		event Action<string> IRecievedAMessage;
	}
}
