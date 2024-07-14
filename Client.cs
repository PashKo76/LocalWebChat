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
		TcpClient client = new TcpClient();
		HttpClient httpClient = new HttpClient();
		CancellationTokenSource cts = new CancellationTokenSource();
		public event Action<string> IRecievedAMessage = (X) => { };
		HttpRequestMessage request;
		public Client(string way, Action<string> Post)
		{
			IRecievedAMessage += Post;
			request = new HttpRequestMessage(HttpMethod.Post, "http://" + way);
			client.Connect(way, 5555);
			Task.Run(() => Checker(cts.Token), cts.Token);
		}
		public void Send(string info)
		{
			info = Environment.MachineName + ": " + info + '\n';
			request.Content = new ByteArrayContent(Encoding.Unicode.GetBytes(info));
			httpClient.Send(request);
		}
		void Checker(CancellationToken ct)
		{
			LinkedList<byte> raw = new LinkedList<byte>();
			using(var str = client.GetStream())
			{
				int helper;
				while (!ct.IsCancellationRequested)
				{
					helper = str.ReadByte();
					if (helper == -1) continue;
					if (helper == '\n')
					{
						IRecievedAMessage.Invoke(Encoding.Unicode.GetString(raw.ToArray()));
						raw.Clear();
					}
					raw.AddLast((byte)helper);
				}
			}
		}
		public void Stop()
		{
			cts.Cancel();
			cts.Dispose();
			client.Close();
		}
	}
}
