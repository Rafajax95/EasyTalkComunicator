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
using System.Threading;

namespace EasyTalk.ClientUI.Views
{
	/// <summary>
	/// Interaction logic for WaitingRoomView.xaml
	/// </summary>
	public partial class WaitingRoomView : Page
	{
		public Frame mainFrame;

		List<UserLabel> userLabels;
		List<RoomLabel> roomLabels;
		RoomRecipient RoomRecipientLabel;
		public RoomView childRoom;

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
			RoomChangingListener();
		}

		private async void SetListener()
		{
			while (Connection.Connected)
			{

				string message = await Connection.Listener();

				if (!String.IsNullOrEmpty(message))
				{
					if(childRoom == null)
						AddMessageToRichBox(message,this.richTextBox);
					else
						AddMessageToRichBox(message, childRoom.richTextBox);
				}


				if (Connection == null)
					break;


				if (Connection.Client != null)
				{
					if (RoomId == Connection.Client.RoomId)
					{
						SetTopInformations(this.TopInfoLB, this.ConnectionStatusLB);
						SetRoomsRecipientLabel(ref this.RoomRecipientLabel, this.UsersSP, this.RoomId);
						SetUserList(this.userLabels, this.UsersSP, RoomId);
						SetRoomsList();
					}

					else if (childRoom != null)
					{
						SetTopInformations(childRoom.TopInfoLB, childRoom.ConnectionStatusLB);
						SetRoomsRecipientLabel(ref childRoom.RoomRecipientLabel, childRoom.UsersSP, childRoom.roomId);
						SetUserList(childRoom.userLabels, childRoom.UsersSP, childRoom.roomId);
					}

				}

			}
			LogoutBT_Click(null, null);
		}

		private void AddMessageToRichBox(string message, RichTextBox target)
		{
			target.AppendText(String.Format("\r{0}", message));
			target.LineDown();
		}

		private void SetRoomsRecipientLabel(ref RoomRecipient RoomRecipientLabel, StackPanel UsersSP, int RoomId)
		{
			if (RoomRecipientLabel == null)
			{
				RoomRecipientLabel = new RoomRecipient(Connection.Rooms.Where(x => x.Id == RoomId).First(),this);
				RoomRecipientLabel.Grid_MouseUp(null, null);
				UsersSP.Children.Add(RoomRecipientLabel);
			}
		}

		private void SetUserList(List<UserLabel> userLabels, StackPanel UsersSP, int RoomId)
		{
			foreach (var user in Connection.AllUsers)
			{
				UserLabel userLabel = userLabels.Where(x => (x.User.Id == user.Id) && (x.User.Id != Connection.Client.Id)).FirstOrDefault();

				if (userLabel == null && user.RoomId == RoomId)
				{
					userLabel = new UserLabel(user, this);
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

			for (int i = 0; i < userLabels.Count; i++)
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

		internal void ClientBackToWaitingRoom()
		{
			childRoom = null;
			if (Connection.Client.Status == Status.Online)
			{
				ChangeStatusBT.Content = "Dostępny";
			}
			else
			{
				ChangeStatusBT.Content = "Zaraz Wracam";
			}
			RoomRecipientLabel.Grid_MouseUp(null, null);
		}

		private void SetRoomsList()
		{
			foreach (var room in Connection.Rooms)
			{
				RoomLabel roomLabel = roomLabels.Where(x => x.Room.Id == room.Id).FirstOrDefault();

				if (roomLabel == null && room.Id != RoomId)
				{
					roomLabel = new RoomLabel(room,Connection);
					roomLabels.Add(roomLabel);
					RoomsSP.Children.Add(roomLabel);
				}
			}


			for (int i = 0; i < roomLabels.Count; i++)
			{
				Room comparatedRoom = Connection.Rooms.Where(x => x.Id == roomLabels[i].Room.Id).FirstOrDefault();

				if (comparatedRoom == null)
				{
					RoomsSP.Children.Remove(roomLabels[i]);
					roomLabels.Remove(roomLabels[i]);
					i--;
				}
			}
		}


		private void SetTopInformations(Label TopInfoLB, Label ConnectionStatusLB)
		{
			string roomName = Connection.Rooms.Where(x => x.Id == Connection.Client.RoomId).First().Name;
			TopInfoLB.Content = String.Format("Zalogowano jako: {0}          IP Serwera: {1}{2}Pokój: {3}", Connection.Client.Name, Connection.Ip, Environment.NewLine, roomName);
			ConnectionStatusLB.Content = String.Format("Twój status: {0}", StatusToString.Convert(Connection.Client.Status));
		}

		public void LogoutBT_Click(object sender, RoutedEventArgs e)
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
			if (Connection.ActualRecipient != null && !String.IsNullOrEmpty(NewMessageTB.Text) && !String.IsNullOrWhiteSpace(NewMessageTB.Text))
			{
				bool existInUsers = false;
				bool existInRooms = false;
				try
				{
					existInUsers = Connection.AllUsers.Exists(x => x.Id == (Connection.ActualRecipient as User).Id);
				}
				catch { }
				try
				{
					existInRooms = Connection.Rooms.Exists(x => x.Id == (Connection.ActualRecipient as Room).Id);
				}
				catch { }

				if (existInUsers || existInRooms)
				{
					try
					{
						Connection.SendTextMessageRequest(NewMessageTB.Text, Connection.ActualRecipient);
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

		private void NewRoomBT_Click(object sender, RoutedEventArgs e)
		{
			Window win = new CreateRoomWindow(Connection);
			win.ShowDialog();
		}

		private void NewMessageTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SendBT_Click(null, null);
			}
		}

		private void OnRoomChanged(Room room)
		{
			if (room.Id != 0)
			{
				this.Dispatcher.Invoke(() =>
				{
					childRoom = new RoomView(this, room);
					mainFrame.Navigate(childRoom);
				});

			}
		}

		private async void RoomChangingListener()
		{
			await Task.Run(() =>
			{
				while (true)
				{
					if (Connection == null)
						break;
					if (Connection.Client != null && childRoom == null)
					{
						if (Connection.Client.RoomId != RoomId && Connection.Client.RoomId != 0)
						{
							Room newRoom = Connection.Rooms.Where(x => x.Id == Connection.Client.RoomId).FirstOrDefault();
							if (newRoom != null)
								OnRoomChanged(newRoom);
						}
					}
					Thread.Sleep(10);
				}
			});
		}
	}
}
