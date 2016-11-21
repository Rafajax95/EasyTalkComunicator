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
using EasyTalk.Utils.Models;
using EasyTalk.Client;
namespace EasyTalk.ClientUI.Views
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : Page
	{
		Frame mainFrame;
		public LoginView(Frame frameReference)
		{
			this.mainFrame = frameReference;
			InitializeComponent();
		}

		private async void LoginBT_Click(object sender, RoutedEventArgs e)
		{
			if (UserNameTB.Text.Length < 3 && String.IsNullOrEmpty(IpTB.Text))
			{
				//TODO: Zabezpieczenie
			}
			else
			{
				Connection conn = new Connection(IpTB.Text, 2000);
				InfoLB.Content = "Łączenie...";
				if(await conn.Connect(UserNameTB.Text))
				{
					mainFrame.Navigate(new WaitingRoomView(mainFrame, conn));
				}
				else
				{
					InfoLB.Content = "Połączenie nieudane!";
				}
				
			}

		}
	}
}
