using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyTalk.Utils.Dictionaries;
using EasyTalk.Utils.Models;
using EasyTalk.Utils;

namespace EasyTalk.Client
{
	public class Connection
	{
		public string Ip { get; }
		public int Port { get; }
		public User Client { get; private set; }
		public Recipient ActualRecipient { get; set; }
		public List<User> AllUsers { get; private set; }
		public List<Room> Rooms { get; private set; }

		private Socket socket;
		private NetworkStream networkStream;
		private StreamReader reader;
		private StreamWriter writer;
		public bool Connected { get; private set; }


		public Connection(string ip, int port)
		{
			Ip = ip;
			Port = port;
			AllUsers = new List<User>();
			Rooms = new List<Room>();

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Connected = false;
		}

		public async Task<bool> Connect(string userName)
		{
			return await Task.Run(() =>
			{
				try
				{
					socket.Connect(IPAddress.Parse(Ip), Port);
					if (socket.Connected)
					{
						Connected = true;
						CreateComponents();
						SendRegistrationUserRequest(userName);

						return true;
					}

					return false;
				}
				catch
				{
					return false;
				}

			});
		}

		public bool Disconnect()
		{
			try
			{
				SendUnregistrationMessage();
				Connected = false;
				reader.Close();
				writer.Close();
				networkStream.Close();
			}
			catch { }

			return true;
		}

		private void CreateComponents()
		{
			networkStream = new NetworkStream(socket);
			reader = new StreamReader(networkStream);
			writer = new StreamWriter(networkStream);
		}

		public async Task<string> Listener()
		{
			return await Task.Run(() =>
			{
				try
				{
					string result;
					result = reader.ReadLine();
					result = ReadMessage(result);
					return result;
				}
				catch
				{
					return null;
				}
			});

		}

		private string ReadMessage(string result)
		{
			MessageModel message = MessageReader.TryReadMessage(result);
			string textMessage;
			switch (message.messageType)
			{
				case MessageTypes.TextMessageResponse:
					textMessage = ReadTextMessageResponse(message);
					break;
				case MessageTypes.RegistrationUserResponse:
					textMessage = ReadRegistrationUserResponse(message);
					break;
				case MessageTypes.ServerStatusMessage:
					textMessage = ReadServerStatusMessage(message);
					break;
				case MessageTypes.RoomStatusMessage:
					textMessage = ReadRoomStatusMessage(message);
					break;
				default:
					textMessage = null;
					break;
			}
			return textMessage;

		}

		private string ReadRoomStatusMessage(MessageModel message)
		{
			Client.RoomId = (message as RoomStatusMessageModel).room.Id;
			SendClientStatus();
			return null;
		}

		private string ReadServerStatusMessage(MessageModel message)
		{
			object locker = new object();
			lock (locker)
			{
				AllUsers = (message as ServerStatusMessageModel).users;
				Rooms = (message as ServerStatusMessageModel).rooms;
				Client = AllUsers.Where(x => x.Id == Client.Id).First();
				AllUsers.Remove(Client);
				return null;
			}

		}


		private string ReadRegistrationUserResponse(MessageModel message)
		{
			int id = (message as RegistrationUserResponseModel).userId;
			int roomID = 0;
			Status status = Status.Online;
			string name = (message as RegistrationUserResponseModel).userName;
			ActualRecipient = Rooms.Where(x => x.Id == 0).First();

			Client = new User(name, id, status, roomID);

			return null;
		}

		private string ReadTextMessageResponse(MessageModel message)
		{
			return (message as TextMessageResponseModel).content;
		}

		public void SendRegistrationUserRequest(string userName)
		{
			writer.WriteLine(MessageWriter.RegistrationUserRequest(userName));
			writer.Flush();
		}

		public void SendClientStatus()
		{
			writer.WriteLine(MessageWriter.UserStatusMessage(Client));
			writer.Flush();
		}

		public void SendTextMessageRequest(string message, Recipient recipient)
		{
			TextMessageType textMessageType;
			int recipientId;

			if (recipient.GetType() == typeof(User))
			{
				textMessageType = TextMessageType.Private;
				recipientId = (recipient as User).Id;
			}
			else
			{
				textMessageType = TextMessageType.Public;
				recipientId = (recipient as Room).Id;
			}

			writer.WriteLine(MessageWriter.TextMessageRequest(textMessageType, Client.Id, recipientId, message));
			writer.Flush();
		}




		public void SendUnregistrationMessage()
		{
			writer.WriteLine(MessageWriter.UnregistrationMessage(Client.Id));
			writer.Flush();
		}

		public void SendCreateRoomRequest(string roomName, string roomPassword)
		{

			writer.WriteLine(MessageWriter.CreateRoomRequest(roomName, roomPassword));
			writer.Flush();
		}

	}
}

