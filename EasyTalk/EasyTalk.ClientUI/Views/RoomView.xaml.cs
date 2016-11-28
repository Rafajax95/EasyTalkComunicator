using EasyTalk.ClientUI.UserControls;
using EasyTalk.Utils;
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

namespace EasyTalk.ClientUI.Views
{
	/// <summary>
	/// Interaction logic for RoomView.xaml
	/// </summary>
	public partial class RoomView : Page
	{
		WaitingRoomView waitingRoomRef;
		public int roomId;
		public List<UserLabel> userLabels;
		public RoomRecipient RoomRecipientLabel;
		public RoomView(WaitingRoomView waitingRoomRef, Room room)
		{
			InitializeComponent();
			this.waitingRoomRef = waitingRoomRef;
			this.roomId = room.Id;
			userLabels = new List<UserLabel>();
			waitingRoomRef.Connection.SendClientStatus();
			if (waitingRoomRef.Connection.Client.Status == Status.RightBack)
				ChangeStatusBT.Content = "Dostępny";
			waitingRoomRef.Connection.ActualRecipient = room;
			MessageRecipientLB.Content = room.Name;

		}


		private void ChangeStatusBT_Click(object sender, RoutedEventArgs e)
		{
			if (waitingRoomRef.Connection.Client == null)
			{
				LogoutBT_Click(null, null);
			}

			else
			{
				if (waitingRoomRef.Connection.Client.Status == Status.Online)
				{
					waitingRoomRef.Connection.Client.Status = Status.RightBack;
					ChangeStatusBT.Content = "Dostępny";
				}
				else
				{
					waitingRoomRef.Connection.Client.Status = Status.Online;
					ChangeStatusBT.Content = "Zaraz Wracam";
				}

				waitingRoomRef.Connection.SendClientStatus();
			}
		}

		private void LogoutBT_Click(object sender, RoutedEventArgs e)
		{
			waitingRoomRef.LogoutBT_Click(sender, e);
		}

		private void SendBT_Click(object sender, RoutedEventArgs e)
		{
			if (waitingRoomRef.Connection.ActualRecipient != null && !String.IsNullOrEmpty(NewMessageTB.Text) && !String.IsNullOrWhiteSpace(NewMessageTB.Text))
			{
				bool existInUsers = false;
				bool existInRooms = false;
				try
				{
					existInUsers = waitingRoomRef.Connection.AllUsers.Exists(x => x.Id == (waitingRoomRef.Connection.ActualRecipient as User).Id);
				}
				catch { }
				try
				{
					existInRooms = waitingRoomRef.Connection.Rooms.Exists(x => x.Id == (waitingRoomRef.Connection.ActualRecipient as Room).Id);
				}
				catch { }

				if (existInUsers || existInRooms)
				{
					try
					{
						waitingRoomRef.Connection.SendTextMessageRequest(NewMessageTB.Text, waitingRoomRef.Connection.ActualRecipient);
						NewMessageTB.Text = String.Empty;
					}
					catch
					{
						LogoutBT_Click(null, null);
					}
				}
				else
				{
					NewMessageTB.Text = "Ten odbiorca jest już niedostępny";
				}
			}
		}

		private void QuitRoomBT_Click(object sender, RoutedEventArgs e)
		{
			waitingRoomRef.Connection.Client.RoomId = 0;
			waitingRoomRef.Connection.SendClientStatus();
			waitingRoomRef.ClientBackToWaitingRoom();
			waitingRoomRef.mainFrame.Navigate(waitingRoomRef);
		}

		private void NewMessageTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SendBT_Click(null, null);
			}
		}

	}
}
