using EasyTalk.Client;
using EasyTalk.ClientUI.Views;
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
		WaitingRoomView parent;

		public RoomRecipient(Room room, WaitingRoomView parent)
		{
			InitializeComponent();
			this.room = room;
			this.parent = parent;
			RoomLB.Content = String.Format("Wszyscy w: {0}", room.Name);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
		}

		public void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			parent.Connection.ActualRecipient = room;

			if (parent.childRoom == null)
				parent.MessageRecipientLB.Content = room.Name;
			else
				parent.childRoom.MessageRecipientLB.Content = room.Name;
		}
	}
}
