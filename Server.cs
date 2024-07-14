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
		HttpListener waiter = new HttpListener();
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		List<TcpClient> clients = new List<TcpClient>();
		public event Action<string> IRecievedAMessage = (X) => { };
		public Server(string way, Action<string> Post)
		{
			IRecievedAMessage += Post;
			listner.Start();
			waiter.Prefixes.Add("http://127.0.0.1:5556");
			waiter.Start();
			Task.Run(() => Pending(tokenSource.Token), tokenSource.Token);
			Task.Run(() => Checker(tokenSource.Token), tokenSource.Token);
		}
		public void Send(string info)
		{
			SubSend("Server:" + info);
		}
		void SubSend(string info)
		{
			string finalForm = info + '\n';
			SendBytes(Encoding.Unicode.GetBytes(finalForm));
		}
		void SendBytes(byte[] info)
		{
			Parallel.ForEach(clients, (X) =>
			{
				try
				{
					using (var s = X.GetStream())
					{
						s.Write(info);
					}
				}
				catch (IOException)
				{
					clients.Remove(X);
					X.Close();
					X.Dispose();
				}
			});
		}
		void Checker(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				var cont = waiter.GetContext();
				CheckContext(cont);
			}
		}
		void CheckContext(HttpListenerContext context)
		{
			using(Stream str = context.Request.InputStream)
			{
				byte[] bytes = new byte[str.Length];
				str.Read(bytes);
				str.Flush();
				SendBytes(bytes);
				IRecievedAMessage.Invoke(Encoding.Unicode.GetString(bytes));
			}
		}
		void Pending(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				//if (!listner.Pending())
				//{
				//	continue;
				//}
				clients.Add(listner.AcceptTcpClient());
			}
		}
		public void Stop()
		{
			tokenSource.Cancel();
			listner.Stop();
			waiter.Stop();
			tokenSource.Dispose();
		}
	}
}
