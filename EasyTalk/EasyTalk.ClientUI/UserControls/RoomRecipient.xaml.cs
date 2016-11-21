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
	/// Interaction logic for RoomRecipient.xaml
	/// </summary>
	public partial class RoomRecipient : UserControl
	{
		Room room;
		Connection connection;
		Label recipientLabel;
		public RoomRecipient(Room room, Connection connection, Label recipientLabel)
		{
			InitializeComponent();
			this.room = room;
			this.connection = connection;
			this.recipientLabel = recipientLabel;
			RoomLB.Content = String.Format("Wszyscy w: {0}", room.Name);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
		}

		public void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			connection.ActualRecipient = room;
			recipientLabel.Content = room.Name;
		}
	}
}
