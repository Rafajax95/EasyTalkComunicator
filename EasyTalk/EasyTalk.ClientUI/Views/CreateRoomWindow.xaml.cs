using EasyTalk.Client;
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
using System.Windows.Shapes;

namespace EasyTalk.ClientUI.Views
{
	/// <summary>
	/// Interaction logic for CreateRoomWindow.xaml
	/// </summary>
	public partial class CreateRoomWindow : Window
	{

		Connection connection;
		public CreateRoomWindow(Connection connection)
		{
			InitializeComponent();
			this.connection = connection;
		}

		private void CancelBT_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void CreateRoomBT_Click(object sender, RoutedEventArgs e)
		{
			if(!String.IsNullOrEmpty(RoomNameTB.Text) && !String.IsNullOrWhiteSpace(RoomNameTB.Text))
			{
				connection.SendCreateRoomRequest(RoomNameTB.Text, PasswordTB.Text);
				this.Close();
			}
		}
	}
}
