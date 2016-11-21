using EasyTalk.Utils;
using EasyTalk.Utils.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.EasyServer
{
	public class Client
	{
		public static List<Client> AllClients = new List<Client>();
		public StreamReader sr;
		public StreamWriter sw;
		Server server;
		User user;

		public Client(NetworkStream stream, Server server)
		{
			this.server = server;
			sw = new StreamWriter(stream);
			sr = new StreamReader(stream);
			AllClients.Add(this);
		}

		public async void RunClientReader(StreamReader sr, StreamWriter sw)
		{
			await Task.Run(() =>
			{
				while (true)
				{
					string message;
					try
					{
						message = sr.ReadLine();
					}
					catch
					{
						object locker = new object();
						lock (locker)
						{
							if (user != null)
								server.Users.Remove(this.user);
							AllClients.Remove(this);

							WriteToAllClients(MessageWriter.ServerStatusMessage(server.Users, server.Rooms));
						}
						break;
					}

					Console.WriteLine(message);
					MessageModel messageModel = MessageReader.TryReadMessage(message);

					switch (messageModel.messageType)
					{
						case MessageTypes.TextMessageRequest:
							ReadTextMessageRequest(messageModel);
							break;
						case MessageTypes.RegistrationUserRequest:
							ReadRegistrationUserRequest(messageModel);
							break;
						case MessageTypes.UnregistrationMessage:
							ReadUnregistrationMessage(messageModel);
							break;
						case MessageTypes.UserStatusMessage:
							ReadUserStatusMessage(messageModel);
							break;
						case MessageTypes.CreateRoomRequest:
							ReadCreateRoomRequest(messageModel);
							break;
					}

					WriteToAllClients(MessageWriter.ServerStatusMessage(server.Users, server.Rooms));
				}
			});
		}

		private void ReadTextMessageRequest(MessageModel message)
		{
			TextMessageRequestModel messageAsTextRequest = message as TextMessageRequestModel;
			Client sender = AllClients.Where(x => x.user.Id == messageAsTextRequest.senderId).FirstOrDefault();
			Client recipient = AllClients.Where(x => x.user.Id == messageAsTextRequest.recipientId).FirstOrDefault();

			string messageToSend = String.Format("{0}: {1}",sender.user.Name,messageAsTextRequest.content);
			string preparedMessage = MessageWriter.TextMessageResponse(sender.user.Id, messageToSend);

			if (messageAsTextRequest.textMessageType == TextMessageType.Private)
			{

				WriteToClient(preparedMessage, sender);
				WriteToClient(preparedMessage, recipient);
			}

			if (messageAsTextRequest.textMessageType == TextMessageType.Public)
			{
				List<Client> recipients = AllClients.Where(x => x.user.RoomId == messageAsTextRequest.recipientId).ToList();
				foreach(var rec in recipients)
				{
					WriteToClient(preparedMessage, rec);
				}
			}
		}
		private void ReadRegistrationUserRequest(MessageModel message)
		{
			object locker = new object();
			int id;
			lock (locker)
			{
				id = GetFreeId(server.Users.Select(x => x.Id).ToList());
			}

			string userName = (message as RegistrationUserRequestModel).userName;
			this.user = new User(userName, id);
			string responsToNewUser = MessageWriter.RegistrationUserResponse(userName, id);

			lock (locker)
			{
				server.Users.Add(this.user);
				sw.WriteLine(MessageWriter.ServerStatusMessage(server.Users, server.Rooms));
				sw.Flush();
				sw.WriteLine(responsToNewUser);
				sw.Flush();
			}


		}

		private void WriteToAllClients(string message)
		{
			object locker = new object();
			foreach (var client in AllClients)
			{
				lock (locker)
				{
					client.sw.WriteLine(message);
					client.sw.Flush();
				}
			}
		}

		private void WriteToClient(string message, Client recipient)
		{
			object locker = new object();
			lock (locker)
			{
				recipient.sw.WriteLine(message);
				recipient.sw.Flush();
			}
		}

		private void ReadUnregistrationMessage(MessageModel message)
		{
			object locker = new object();
			int userToRemoveId = (message as UnregistrationMessageModel).userId;

			lock (locker)
			{
				server.Users.Remove(server.Users.Where(x => x.Id == userToRemoveId).First());
				AllClients.Remove(this);
			}

			sw.Close();
			sr.Close();

		}
		private void ReadUserStatusMessage(MessageModel message)
		{
			user.Status = (message as UserStatusMessageModel).user.Status;
			user.RoomId = (message as UserStatusMessageModel).user.RoomId;
			user.Name = (message as UserStatusMessageModel).user.Name;
		}
		private void ReadCreateRoomRequest(MessageModel message)
		{
			//TODO: Uzupełnić przy wdrażaniu dodawania roomu.
		}

		private int GetFreeId(List<int> busyIds)
		{
			int proposedId = 0;
			bool proposedIdIsFree = false;

			do
			{
				if (busyIds.Contains(proposedId))
					proposedId++;
				else
					proposedIdIsFree = true;

			} while (!proposedIdIsFree);

			return proposedId;
		}
	}
}

