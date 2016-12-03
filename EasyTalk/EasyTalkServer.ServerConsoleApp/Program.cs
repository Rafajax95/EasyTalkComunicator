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
		static Server server;
		static void Main(string[] args)
		{
			Console.WriteLine("Chcesz wpisać ip czy spróbować uruchamiania automatycznego?");
			Console.WriteLine("1.Chcę wpisać.");
			Console.WriteLine("2.Połącz automatycznie.");
			bool end = false;
			do
			{
				int choose = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
				Console.WriteLine();
				switch (choose)
				{
					case 1:
						ChooseConfiguration();
						end = true;
						break;
					case 2:
						RunDefaultServer();
						end = true;
						break;
					default:
						Console.WriteLine("Błędny wybór! Spróbuj jeszcze raz!");
						end = false;
						break;
				}

			} while (!end);


			server.RunServer();
			Console.ReadLine();
		}

		private static void RunDefaultServer()
		{
			server = new Server();
		}

		private static void ChooseConfiguration()
		{
			Console.Write("Podaj IP: ");
			string ip = Console.ReadLine();
			int port = 2000;
			server = new Server(ip, port);
		}
	}
}
