using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebChat
{
	internal class Client : INetworkHandler
	{
		Mutex mutex = new Mutex();
		NetworkStream client;
		CancellationTokenSource cts = new CancellationTokenSource();
		public event Action<string> IRecievedAMessage = (X) => { };
		public Client(string way, Action<string> Post)
		{
			IRecievedAMessage += (X) => { mutex.WaitOne(); Post.Invoke(X); mutex.ReleaseMutex(); };
			client = new TcpClient(way, 5555).GetStream();
			Task.Run(() => Checker(cts.Token), cts.Token);
		}
		public void Send(string info)
		{
			info = Environment.MachineName + ": " + info;
			SendBytes(Encoding.Unicode.GetBytes(info));
		}
		void SendBytes(byte[] bytes)
		{
			client.Write(bytes);
		}
		void Checker(CancellationToken ct)
		{
			LinkedList<byte> raw = new LinkedList<byte>();
			string helper;
			while (!ct.IsCancellationRequested)
			{
				mutex.WaitOne();
				if (!client.DataAvailable) continue;
				while (client.DataAvailable)
				{
					raw.AddLast((byte)client.ReadByte());
				}
				helper = Encoding.Unicode.GetString(raw.ToArray());
				if(helper == "Disconnect")
				{
					SubStop();
					break;
				}
				IRecievedAMessage.Invoke(helper);
				raw.Clear();
				mutex.ReleaseMutex();
			}
		}
		public void Stop()
		{
			SendBytes(Encoding.Unicode.GetBytes("Disconnect"));
			SubStop();
		}
		void SubStop()
		{
			IRecievedAMessage.Invoke("Disconnected");
			cts.Cancel();
			cts.Dispose();
			client.Close();
			mutex.Dispose();
		}
	}
}
