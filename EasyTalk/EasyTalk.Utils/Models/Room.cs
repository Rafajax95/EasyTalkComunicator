using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Utils.Models
{
	public class Room : Recipient
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }
		public Room(int id, string name, string password)
		{
			Id = id;
			Name = name;
			Password = password;
			if(Password==null)
			{
				Password = String.Empty;
			}
		}
	}
}
