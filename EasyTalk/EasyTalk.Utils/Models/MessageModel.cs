using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Utils.Models
{
	public abstract class MessageModel
	{
		public MessageTypes messageType;
		public MessageModel(MessageTypes messageType)
		{
			this.messageType = messageType;
		}

	}

	public class TextMessageRequestModel : MessageModel
	{
		public TextMessageType textMessageType;
		public int senderId;
		public int recipientId;
		public string content;

		public TextMessageRequestModel(MessageTypes messageType ,TextMessageType textMessageType, int senderId, int recipientId, string content) :base(messageType)
		{
			this.textMessageType = textMessageType;
			this.senderId = senderId;
			this.recipientId = recipientId;
			this.content = content;
		}
	}

	public class TextMessageResponseModel : MessageModel
	{
		public int senderId;
		public string content;
		public TextMessageResponseModel(MessageTypes messageType, int senderId, string content) : base(messageType)
		{
			this.senderId = senderId;
			this.content = content;
		}
	}

	public class RegistrationUserRequestModel : MessageModel
	{
		public string userName;

		public RegistrationUserRequestModel(MessageTypes messageType, string userName) : base(messageType)
		{
			this.userName = userName;
		}
	}

	public class RegistrationUserResponseModel : MessageModel
	{
		public string userName;
		public int userId;

		public RegistrationUserResponseModel(MessageTypes messageType, string userName, int userId) : base(messageType)
		{
			this.userName = userName;
			this.userId = userId;
		}
	}

	public class UnregistrationMessageModel : MessageModel
	{
		public int userId;
		public UnregistrationMessageModel(MessageTypes messageType, int userId) : base(messageType)
		{
			this.userId = userId;
		}
	}

	public class UserStatusMessageModel : MessageModel
	{
		public User user;
		public UserStatusMessageModel(MessageTypes messageType, User user) : base(messageType)
		{
			this.user = user;
		}
	}

	public class RoomStatusMessageModel : MessageModel
	{
		public Room room;
		public RoomStatusMessageModel(MessageTypes messageType, Room room) : base(messageType)
		{
			this.room = room;
		}
	}

	public class CreateRoomRequestModel : MessageModel
	{
		public string roomName;
		public string roomPassword;
		public CreateRoomRequestModel(MessageTypes messageType, string roomName, string roomPassword) : base(messageType)
		{
			this.roomName = roomName;
			this.roomPassword = roomPassword;
		}
	}

	public class ServerStatusMessageModel : MessageModel
	{
		public List<User> users;
		public List<Room> rooms;
		public  ServerStatusMessageModel(MessageTypes messageType, List<User> users, List<Room> rooms) : base(messageType)
		{
			this.users = users;
			this.rooms = rooms;
		}
	}

}
