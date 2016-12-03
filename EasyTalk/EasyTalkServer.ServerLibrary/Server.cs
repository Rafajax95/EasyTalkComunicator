using EasyTalk.Utils;
using EasyTalk.Utils.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.EasyServer
{
	public class Server
	{
		public List<Room> Rooms { get; private set; }
		public List<User> Users { get; private set; }
		public string Ip { get; private set; }

		public int Port { get; private set; }

		private Socket mainSocket;
		private IPAddress address;
		private IPEndPoint endPoint;

		public Server(int port = 2000)
		{
			Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x => x.ToString()).First(x => x.StartsWith("192") || x.StartsWith("172") || x.StartsWith("10"));
			Port = port;
			Rooms = new List<Room>();
			Users = new List<User>();
			Rooms.Add(new Room(0, "Poczekalnia", null));
		}
		public Server(string ip, int port)
		{
			Ip = ip;
			Port = port;
			Rooms = new List<Room>();
			Users = new List<User>();
			Rooms.Add(new Room(0, "Poczekalnia", null));
		}

		public void RunServer()
		{
			mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			address = IPAddress.Parse(Ip);
			endPoint = new IPEndPoint(address, Port);
			mainSocket.Bind(endPoint);
			mainSocket.Listen(10);
			RunListining();
			Console.WriteLine("Nasłuch na adresie: {0}", Ip);
		}

		private async void RunListining()
		{
			await Task.Run(() =>
			{
				while (true)
				{
					Socket newClientSocket = mainSocket.Accept();
					Console.WriteLine("Połączyłem! xD");
					NetworkStream stream = new NetworkStream(newClientSocket);
					StreamWriter sw = new StreamWriter(stream);
					StreamReader sr = new StreamReader(stream);
					Client newClient = new Client(stream, this);
					newClient.RunClientReader(sr, sw);
				}
			});
		}


	}
}
