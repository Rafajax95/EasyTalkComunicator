using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using EasyTalk.EasyServer;
namespace EasyTalk.ServerConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Server server = new Server();
			server.RunServer();
			Console.ReadLine();
		}
		//static void Main(string[] args)
		//{

		//	Console.WriteLine("Przygotowuję serwer...");
		//	Socket inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		//	string ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x => x.ToString()).First(x => x.StartsWith("192"));
		//	Console.WriteLine("Nasłuch na adresie: " + ip);
		//	IPAddress address = IPAddress.Parse(ip);
		//	IPEndPoint endPoint = new IPEndPoint(address, 2000);
		//	inputSocket.Bind(endPoint);
		//	inputSocket.Listen(10);
		//	Console.WriteLine("Oczekuję na połączenie...");
		//	Socket clientSocket = inputSocket.Accept();
		//	NetworkStream stream = new NetworkStream(clientSocket);
		//	StreamWriter sw = new StreamWriter(stream);
		//	Console.WriteLine("Połączono!");
		//	while (true)
		//	{
		//		string massage;
		//		FileStream fs1 = new FileStream(@"C:\Users\dzub\Desktop\message1.txt", FileMode.Open, FileAccess.Read);
		//		StreamReader sr1 = new StreamReader(fs1);
		//		massage = sr1.ReadToEnd();
		//		fs1.Close();

		//		sw.WriteLine(massage);
		//		sw.Flush();

		//		FileStream fs2 = new FileStream(@"C:\Users\dzub\Desktop\message2.txt", FileMode.Open, FileAccess.Read);
		//		StreamReader sr2 = new StreamReader(fs2);
		//		massage = sr2.ReadToEnd();
		//		fs2.Close();

		//		sw.WriteLine(massage);
		//		sw.Flush();

		//	}
		//	sw.Close();
		//	stream.Close();
		//	clientSocket.Close();
		//	inputSocket.Close();
		//	Console.ReadLine();
		//}
	}
}
