using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTalk.Utils.Dictionaries;

namespace EasyTalk.Utils.Models
{

	public class User : Recipient
	{
		public string Name { get; set; }
		public Status Status { get; set; }
		public int RoomId { get; set; }

		public int Id { get; set; }

		public User(string name, int userId)
		{
			Name = name;
			Id = userId;
			Status = Status.Online;
			RoomId = 0;
		}

		public User(string name, int userId, Status status, int roomId)
		{
			Name = name;
			Status = status;
			Id = userId;
			RoomId = roomId;
		}

	}

}
