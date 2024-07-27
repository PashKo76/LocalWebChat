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
		public Server(Action<string> Post)
		{
			IRecievedAMessage += Post;
			listner.Start();
			Task.Run(() => Cycle(tokenSource.Token), tokenSource.Token);
		}
		void Cycle(CancellationToken ct)
		{
			Pending(ct);
			Checker(ct);
		}
		public void Send(string info)
		{
			SendBytes(Encoding.Unicode.GetBytes("Server: " + info));
		}
		void SendBytes(byte[] info)
		{
			if (clients.Count == 0) return;
			Parallel.ForEach(clients, (X) =>
			{
				X.Write(info);
			});
		}
		void Checker(CancellationToken ct)
		{
			if (clients.Count == 0) return;
			LinkedList<byte> bytes = new LinkedList<byte>();
			string helper;
			foreach (var s in clients)
			{
				if (!s.DataAvailable) continue;
				while (s.DataAvailable)
				{
					bytes.AddLast((byte)s.ReadByte());
				}
				helper = Encoding.Unicode.GetString(bytes.ToArray());
				if (helper == "Disconnect")
				{
					clients.Remove(s);
					IRecievedAMessage.Invoke("User Disconnected");
				}
				else
				{
					IRecievedAMessage.Invoke(helper);
					SendBytes(bytes.ToArray());
				}
				bytes.Clear();
			}
		}
		void Pending(CancellationToken ct)
		{
			while (listner.Pending())
			{
				clients.Add(listner.AcceptTcpClient().GetStream());
				IRecievedAMessage.Invoke("User Connected");
			}
		}
		public void Stop()
		{
			tokenSource.Cancel();
			SendBytes(Encoding.Unicode.GetBytes("Disconnect"));
			foreach (var s in clients)
			{
				s.Close();
			}
			listner.Stop();
			tokenSource.Dispose();
		}
	}
}
