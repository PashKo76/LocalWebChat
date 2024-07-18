using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebChat
{
	internal class Server : INetworkHandler
	{
		TcpListener listner = new TcpListener(IPAddress.Any, 5555);
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		List<NetworkStream> clients = new List<NetworkStream>();
		public event Action<string> IRecievedAMessage = (X) => { };
		Mutex mutex = new Mutex();
		public Server(string way, Action<string> Post)
		{
			IRecievedAMessage += (X) => { mutex.WaitOne(); Post.Invoke(X); mutex.ReleaseMutex(); };
			listner.Start();
			Task.Run(() => Pending(tokenSource.Token), tokenSource.Token);
			Task.Run(() => Checker(tokenSource.Token), tokenSource.Token);
		}
		public void Send(string info)
		{
			SendBytes(Encoding.Unicode.GetBytes("Server:" + info));
		}
		void SendBytes(byte[] info)
		{
			mutex.WaitOne();
			Parallel.ForEach(clients, (X) =>
			{
				X.Write(info);
			});
			mutex.ReleaseMutex();
		}
		void Checker(CancellationToken ct)
		{
			LinkedList<byte> bytes = new LinkedList<byte>();
			string helper;
			while (!ct.IsCancellationRequested)
			{
				mutex.WaitOne();
				foreach(var s in clients)
				{
					if (!s.DataAvailable) continue;
					while (s.DataAvailable)
					{
						bytes.AddLast((byte)s.ReadByte());
					}
					helper = Encoding.Unicode.GetString(bytes.ToArray());
					if(helper == "Disconnect")
					{
						clients.Remove(s);
					}
					IRecievedAMessage.Invoke(helper);
					SendBytes(bytes.ToArray());
					bytes.Clear();
				}
				mutex.ReleaseMutex();
			}
		}
		void Pending(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				mutex.WaitOne();
				clients.Add(listner.AcceptTcpClient().GetStream());
				mutex.ReleaseMutex();
			}
		}
		public void Stop()
		{
			tokenSource.Cancel();
			SendBytes(Encoding.Unicode.GetBytes("Disconnect"));
			foreach(var s in clients)
			{
				s.Close();
			}
			listner.Stop();
			tokenSource.Dispose();
			mutex.Dispose();
		}
	}
}
