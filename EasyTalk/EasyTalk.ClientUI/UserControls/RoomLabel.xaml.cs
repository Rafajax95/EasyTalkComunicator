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

namespace EasyTalk.ClientUI.UserControls
{
	/// <summary>
	/// Interaction logic for RoomLabel.xaml
	/// </summary>
	public partial class RoomLabel : UserControl
	{
		public Room Room;
		private Connection connection;
		public RoomLabel(Room room,Connection connection)
		{
			Room = room;
			InitializeComponent();
			this.connection = connection;
			RoomNameLB.Content = room.Name;
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
		}

		private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(e.ClickCount >= 2)
			{
				connection.Client.RoomId = Room.Id;
				connection.SendClientStatus();
			}
		}
	}
}
