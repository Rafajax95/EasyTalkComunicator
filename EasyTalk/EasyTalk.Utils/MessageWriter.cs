using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EasyTalk.Utils.Models;
using System.Xml;
using EasyTalk.Utils.Dictionaries;

namespace EasyTalk.Utils
{
	public class MessageWriter
	{

		public static string TextMessageRequest(TextMessageType textMessageType, int senderId, int recipientId, string content)
		{

			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.TextMessageRequest).ToString();
			Message.AppendChild(MessageType);

			XmlElement TextMessageType = document.CreateElement(MessageTags.TextMessageType);
			TextMessageType.InnerText = ((int)textMessageType).ToString();
			Message.AppendChild(TextMessageType);

			XmlElement SenderId = document.CreateElement(MessageTags.SenderId);
			SenderId.InnerText = senderId.ToString();
			Message.AppendChild(SenderId);

			XmlElement RecipientId = document.CreateElement(MessageTags.RecipientId);
			RecipientId.InnerText = recipientId.ToString();
			Message.AppendChild(RecipientId);

			XmlElement Content = document.CreateElement(MessageTags.Content);
			Content.InnerText = content;
			Message.AppendChild(Content);

			return document.OuterXml;
		}
		public static string TextMessageResponse(int senderId, string content)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.TextMessageResponse).ToString();
			Message.AppendChild(MessageType);

			XmlElement SenderId = document.CreateElement(MessageTags.SenderId);
			SenderId.InnerText = senderId.ToString();
			Message.AppendChild(SenderId);

			XmlElement Content = document.CreateElement(MessageTags.Content);
			Content.InnerText = content;
			Message.AppendChild(Content);

			return document.OuterXml;
		}
		public static string RegistrationUserRequest(string userName)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.RegistrationUserRequest).ToString();
			Message.AppendChild(MessageType);

			XmlElement NewUserName = document.CreateElement(MessageTags.UserName);
			NewUserName.InnerText = userName;
			Message.AppendChild(NewUserName);

			return document.OuterXml;
		}
		public static string RegistrationUserResponse(string userName, int userId)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.RegistrationUserResponse).ToString();
			Message.AppendChild(MessageType);

			XmlElement NewUserName = document.CreateElement(MessageTags.UserName);
			NewUserName.InnerText = userName;
			Message.AppendChild(NewUserName);

			XmlElement NewUserId = document.CreateElement(MessageTags.UserId);
			NewUserId.InnerText = userId.ToString();
			Message.AppendChild(NewUserId);


			return document.OuterXml;
		}
		public static string UnregistrationMessage(int userId)
		{

			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.UnregistrationMessage).ToString();
			Message.AppendChild(MessageType);


			XmlElement UserId = document.CreateElement(MessageTags.UserId);
			UserId.InnerText = userId.ToString();
			Message.AppendChild(UserId);


			return document.OuterXml;
		}
		public static string UserStatusMessage(User user)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.UserStatusMessage).ToString();
			Message.AppendChild(MessageType);

			XmlElement UserName = document.CreateElement(MessageTags.UserName);
			UserName.InnerText = user.Name;
			Message.AppendChild(UserName);

			XmlElement UserId = document.CreateElement(MessageTags.UserId);
			UserId.InnerText = user.Id.ToString();
			Message.AppendChild(UserId);

			XmlElement UserStatus = document.CreateElement(MessageTags.UserStatus);
			UserStatus.InnerText = ((int)user.Status).ToString();
			Message.AppendChild(UserStatus);

			XmlElement RoomId = document.CreateElement(MessageTags.RoomId);
			RoomId.InnerText = user.RoomId.ToString();
			Message.AppendChild(RoomId);

			return document.OuterXml;
		}
		public static string ServerStatusMessage(List<User> users, List<Room> rooms)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.ServerStatusMessage).ToString();
			Message.AppendChild(MessageType);

			XmlElement Users = document.CreateElement(MessageTags.Users);
			Message.AppendChild(Users);

			XmlElement Rooms = document.CreateElement(MessageTags.Rooms);
			Message.AppendChild(Rooms);

			foreach (var user in users)
			{
				XmlElement User = document.CreateElement(MessageTags.User);
				Users.AppendChild(User);

				XmlElement UserName = document.CreateElement(MessageTags.UserName);
				UserName.InnerText = user.Name;
				User.AppendChild(UserName);

				XmlElement UserId = document.CreateElement(MessageTags.UserId);
				UserId.InnerText = user.Id.ToString();
				User.AppendChild(UserId);

				XmlElement UserStatus = document.CreateElement(MessageTags.UserStatus);
				UserStatus.InnerText = ((int)user.Status).ToString();
				User.AppendChild(UserStatus);

				XmlElement RoomId = document.CreateElement(MessageTags.RoomId);
				RoomId.InnerText = user.RoomId.ToString();
				User.AppendChild(RoomId);


			}
			foreach (var room in rooms)
			{
				XmlElement Room = document.CreateElement(MessageTags.Room);
				Rooms.AppendChild(Room);

				XmlElement RoomName = document.CreateElement(MessageTags.RoomName);
				RoomName.InnerText = room.Name;
				Room.AppendChild(RoomName);

				XmlElement RoomPassword = document.CreateElement(MessageTags.Password);
				RoomPassword.InnerText = room.Password;
				Room.AppendChild(RoomPassword);

				XmlElement RoomId = document.CreateElement(MessageTags.RoomId);
				RoomId.InnerText = room.Id.ToString();
				Room.AppendChild(RoomId);
			}

			return document.OuterXml;
		}
		public static string CreateRoomRequest(string RoomName, string RoomPassword)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.CreateRoomRequest).ToString();
			Message.AppendChild(MessageType);

			XmlElement NewRoomName = document.CreateElement(MessageTags.RoomName);
			NewRoomName.InnerText = RoomName;
			Message.AppendChild(NewRoomName);

			XmlElement NewRoomPassword = document.CreateElement(MessageTags.Password);
			NewRoomPassword.InnerText = RoomPassword;
			Message.AppendChild(NewRoomPassword);

			return document.OuterXml;
		}
		public static string RoomStatusMessage(Room room)
		{
			XmlDocument document = new XmlDocument();

			XmlElement Message = document.CreateElement(MessageTags.Message);
			document.AppendChild(Message);

			XmlElement MessageType = document.CreateElement(MessageTags.MessageType);
			MessageType.InnerText = ((int)MessageTypes.RoomStatusMessage).ToString();
			Message.AppendChild(MessageType);

			XmlElement RoomName = document.CreateElement(MessageTags.RoomName);
			RoomName.InnerText = room.Name;
			Message.AppendChild(RoomName);

			XmlElement RoomPassword = document.CreateElement(MessageTags.Password);
			RoomPassword.InnerText = room.Password;
			Message.AppendChild(RoomPassword);

			XmlElement RoomId = document.CreateElement(MessageTags.RoomId);
			RoomId.InnerText = room.Id.ToString();
			Message.AppendChild(RoomId);

			return document.OuterXml;
		}


		
	}

	public enum MessageTypes
	{
		TextMessageRequest,
		TextMessageResponse,
		RegistrationUserRequest,
		RegistrationUserResponse,
		UnregistrationMessage,
		UserStatusMessage,
		ServerStatusMessage,
		CreateRoomRequest,
		RoomStatusMessage
	}
	public enum TextMessageType
	{
		Public,
		Private
	}
}
