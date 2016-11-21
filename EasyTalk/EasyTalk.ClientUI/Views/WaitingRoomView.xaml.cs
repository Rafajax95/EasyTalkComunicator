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
using EasyTalk.Utils;
using EasyTalk.ClientUI.UserControls;

namespace EasyTalk.ClientUI.Views
{
	/// <summary>
	/// Interaction logic for WaitingRoomView.xaml
	/// </summary>
	public partial class WaitingRoomView : Page
	{
		private Frame mainFrame;

		List<UserLabel> userLabels;
		List<RoomLabel> roomLabels;
		RoomRecipient RoomRecipientLabel;
		int RoomId = 0;
		bool IsConnectionLostMessageShowed;
		public Connection Connection { get; set; }

		public WaitingRoomView(Frame mainFrame, Connection conn)
		{
			InitializeComponent();
			this.mainFrame = mainFrame;
			this.Connection = conn;
			IsConnectionLostMessageShowed = false;
			userLabels = new List<UserLabel>();
			roomLabels = new List<RoomLabel>();

			SetListener();
		}

		private async void SetListener()
		{
			while (Connection.Connected)
			{
				string message = await Connection.Listener();

				if (!String.IsNullOrEmpty(message))
					AddMessageToRichBox(message);

				if (Connection == null)
					break;


				if (Connection.Client != null)
				{
					SetTopInformations();
					
					SetRoomsList();
					if (RoomRecipientLabel == null)
					{
						RoomRecipientLabel = new RoomRecipient(Connection.Rooms.Where(x => x.Id == RoomId).First(), Connection, MessageRecipientLB);
						RoomRecipientLabel.Grid_MouseUp(null, null);
						UsersSP.Children.Add(RoomRecipientLabel);
					}
					SetUserList();
				}

			}
			LogoutBT_Click(null, null);
		}

		private void AddMessageToRichBox(string message)
		{
			richTextBox.AppendText(String.Format("\r{0}", message));
			richTextBox.LineDown();
		}

		private void SetUserList()
		{
			foreach (var user in Connection.AllUsers)
			{
				UserLabel userLabel = userLabels.Where(x => (x.User.Id == user.Id) && (x.User.Id != Connection.Client.Id)).FirstOrDefault();

				if (userLabel == null && user.RoomId == RoomId)
				{
					userLabel = new UserLabel(user, Connection, MessageRecipientLB);
					userLabels.Add(userLabel);
					UsersSP.Children.Add(userLabel);
				}
				else
				{
					if (user.RoomId == RoomId)
					{
						userLabel.UpdateContent(user);
					}
					else
					{
						UsersSP.Children.Remove(userLabel);
					}
				}
			}

			for(int i=0;i<userLabels.Count;i++)
			{
				User comparatedUser = Connection.AllUsers.Where(x => x.Id == userLabels[i].User.Id && x.RoomId == userLabels[i].User.RoomId).FirstOrDefault();

				if (comparatedUser == null)
				{
					UsersSP.Children.Remove(userLabels[i]);
					userLabels.Remove(userLabels[i]);
					i--;
				}
			}


		}

		private void SetRoomsList()
		{
			foreach (var room in Connection.Rooms)
			{
				RoomLabel roomLabel = roomLabels.Where(x => x.Room.Id == room.Id).FirstOrDefault();

				if (roomLabel == null && room.Id != RoomId)
				{
					roomLabel = new RoomLabel(room);
					roomLabels.Add(roomLabel);
					RoomsSP.Children.Add(roomLabel);
				}
			}

			foreach (var roomToCheck in roomLabels)
			{
				Room comparatedRoom = Connection.Rooms.Where(x => x.Id == roomToCheck.Room.Id).FirstOrDefault();

				if (comparatedRoom == null)
				{
					roomLabels.Remove(roomToCheck);
					RoomsSP.Children.Remove(roomToCheck);
				}
			}
		}


		private void SetTopInformations()
		{
			TopInfoLB.Content = String.Format("Zalogowano jako: {0}          IP Serwera: {1}{2}Pokój: Poczekalnia", Connection.Client.Name, Connection.Ip, Environment.NewLine);
			ConnectionStatusLB.Content = String.Format("Twój status: {0}", StatusToString.Convert(Connection.Client.Status));
		}

		private void LogoutBT_Click(object sender, RoutedEventArgs e)
		{

			if (Connection != null)
				Connection.Disconnect();

			Connection = null;
			
			mainFrame.Navigate(new LoginView(mainFrame));
			if (sender == null && !IsConnectionLostMessageShowed)
			{
				IsConnectionLostMessageShowed = true;
				MessageBox.Show("Utracono połączenie z serwerem!");
				
			}
		}

		private void ChangeStatusBT_Click(object sender, RoutedEventArgs e)
		{
			if (Connection.Client == null)
			{
				LogoutBT_Click(null, null);
			}

			else
			{
				if (Connection.Client.Status == Status.Online)
				{
					Connection.Client.Status = Status.RightBack;
					ChangeStatusBT.Content = "Dostępny";
				}
				else
				{
					Connection.Client.Status = Status.Online;
					ChangeStatusBT.Content = "Zaraz Wracam";
				}

				Connection.SendClientStatus();
			}

		}

		private void SendBT_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Connection.SendTextMessageRequest(NewMessageTB.Text, Connection.ActualRecipient);
			}
			catch
			{
				LogoutBT_Click(null, null);
			}
			
		}
	}
}
