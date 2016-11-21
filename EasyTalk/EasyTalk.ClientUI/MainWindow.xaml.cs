using EasyTalk.Client;
using EasyTalk.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTalk.ClientUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		//	Connection conn = new Connection("192.168.9.16", 2000, "Daniel");
		//	conn.Connect();
		//	conn.Client = new User("Daniel", 0);
		//	mainFrame.Navigate(new Views.WaitingRoomView(mainFrame, conn));
			mainFrame.Navigate(new Views.LoginView(mainFrame));
		}
	}
}
