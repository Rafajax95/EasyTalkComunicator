using EasyTalk.Utils.Dictionaries;
using EasyTalk.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyTalk.Utils
{
	public class MessageReader
	{
		public static MessageModel TryReadMessage(string input)
		{
			XmlDocument message = new XmlDocument();
			message.LoadXml(input);

			MessageModel result;

			MessageTypes messageType = (MessageTypes)Int32.Parse(message.GetElementsByTagName(MessageTags.MessageType).Item(0).InnerText);

			switch (messageType)
			{
				case MessageTypes.TextMessageRequest:
					result = ReadTextMessageRequest(messageType, message);
					break;
				case MessageTypes.TextMessageResponse:
					result = ReadTextMessageResponse(messageType, message);
					break;
				case MessageTypes.RegistrationUserRequest:
					result = ReadRegistrationUserRequest(messageType, message);
					break;
				case MessageTypes.RegistrationUserResponse:
					result = ReadRegistrationUserResponse(messageType, message);
					break;
				case MessageTypes.UnregistrationMessage:
					result = ReadUnregistrationMessage(messageType, message);
					break;
				case MessageTypes.UserStatusMessage:
					result = ReadUserStatusMessage(messageType, message);
					break;
				case MessageTypes.ServerStatusMessage:
					result = ReadServerStatusMessage(messageType, message);
					break;
				case MessageTypes.CreateRoomRequest:
					result = ReadCreateRoomRequest(messageType, message);
					break;
				case MessageTypes.RoomStatusMessage:
					result = ReadRoomStatusMessage(messageType, message);
					break;
				default:
					result = null;
					break;

			}
			return result;
		}
		private static TextMessageRequestModel ReadTextMessageRequest(MessageTypes messageType, XmlDocument message)
		{
			TextMessageType textMessageType = (TextMessageType)Int32.Parse(message.GetElementsByTagName(MessageTags.TextMessageType).Item(0).InnerText);
			int senderId = Int32.Parse(message.GetElementsByTagName(MessageTags.SenderId).Item(0).InnerText);
			int recipientId = Int32.Parse(message.GetElementsByTagName(MessageTags.RecipientId).Item(0).InnerText);
			string content = message.GetElementsByTagName(MessageTags.Content).Item(0).InnerText;

			return new TextMessageRequestModel(messageType, textMessageType, senderId, recipientId, content);
		}

		private static TextMessageResponseModel ReadTextMessageResponse(MessageTypes messageType, XmlDocument message)
		{
			int senderId = Int32.Parse(message.GetElementsByTagName(MessageTags.SenderId).Item(0).InnerText);
			string content = message.GetElementsByTagName(MessageTags.Content).Item(0).InnerText;

			return new TextMessageResponseModel(messageType, senderId, content);
		}

		private static RegistrationUserRequestModel ReadRegistrationUserRequest(MessageTypes messageType, XmlDocument message)
		{
			string userName = message.GetElementsByTagName(MessageTags.UserName).Item(0).InnerText;

			return new RegistrationUserRequestModel(messageType, userName);
		}

		private static RegistrationUserResponseModel ReadRegistrationUserResponse(MessageTypes messageType, XmlDocument message)
		{
			string userName = message.GetElementsByTagName(MessageTags.UserName).Item(0).InnerText;
			int userId = Int32.Parse(message.GetElementsByTagName(MessageTags.UserId).Item(0).InnerText);

			return new RegistrationUserResponseModel(messageType, userName, userId);
		}

		private static UnregistrationMessageModel ReadUnregistrationMessage(MessageTypes messageType, XmlDocument message)
		{
			int userId = Int32.Parse(message.GetElementsByTagName(MessageTags.UserId).Item(0).InnerText);

			return new UnregistrationMessageModel(messageType, userId);
		}

		private static UserStatusMessageModel ReadUserStatusMessage(MessageTypes messageType, XmlDocument message)
		{
			string name = message.GetElementsByTagName(MessageTags.UserName).Item(0).InnerText;
			Status status = (Status)Int32.Parse(message.GetElementsByTagName(MessageTags.UserStatus).Item(0).InnerText);
			int userId = Int32.Parse(message.GetElementsByTagName(MessageTags.UserId).Item(0).InnerText);
			int roomId = Int32.Parse(message.GetElementsByTagName(MessageTags.RoomId).Item(0).InnerText);

			return new UserStatusMessageModel(messageType, new User(name, userId, status, roomId));
		}

		private static CreateRoomRequestModel ReadCreateRoomRequest(MessageTypes messageType, XmlDocument message)
		{

			string roomName = message.GetElementsByTagName(MessageTags.RoomName).Item(0).InnerText;
			string roomPassword = message.GetElementsByTagName(MessageTags.Password).Item(0).InnerText;

			return new CreateRoomRequestModel(messageType, roomName, roomPassword);
		}

		private static RoomStatusMessageModel ReadRoomStatusMessage(MessageTypes messageType, XmlDocument message)
		{
			int id = Int32.Parse(message.GetElementsByTagName(MessageTags.RoomId).Item(0).InnerText);
			string name = message.GetElementsByTagName(MessageTags.RoomName).Item(0).InnerText;
			string password = message.GetElementsByTagName(MessageTags.Password).Item(0).InnerText;

			return new RoomStatusMessageModel(messageType, new Room(id, name, password));
		}

		private static ServerStatusMessageModel ReadServerStatusMessage(MessageTypes messageType, XmlDocument message)
		{
			List<User> users = new List<User>();
			List<Room> rooms = new List<Room>();

			XmlNodeList usersXml = message.GetElementsByTagName(MessageTags.User);
			XmlNodeList roomsXml = message.GetElementsByTagName(MessageTags.Room);

			foreach (var user in usersXml)
			{
				string name = (user as XmlElement).GetElementsByTagName(MessageTags.UserName).Item(0).InnerText;
				Status status = (Status)Int32.Parse((user as XmlElement).GetElementsByTagName(MessageTags.UserStatus).Item(0).InnerText);
				int userId = Int32.Parse((user as XmlElement).GetElementsByTagName(MessageTags.UserId).Item(0).InnerText);
				int roomId = Int32.Parse((user as XmlElement).GetElementsByTagName(MessageTags.RoomId).Item(0).InnerText);

				users.Add(new User(name, userId, status, roomId));
			}

			foreach (var room in roomsXml)
			{
				int id = Int32.Parse((room as XmlElement).GetElementsByTagName(MessageTags.RoomId).Item(0).InnerText);
				string name = (room as XmlElement).GetElementsByTagName(MessageTags.RoomName).Item(0).InnerText;
				string password = (room as XmlElement).GetElementsByTagName(MessageTags.Password).Item(0).InnerText;

				rooms.Add(new Room(id, name, password));
			}

			return new ServerStatusMessageModel(messageType, users, rooms);
		}

	}
}
