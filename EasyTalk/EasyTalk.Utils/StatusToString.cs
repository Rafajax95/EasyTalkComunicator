using EasyTalk.Utils.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Utils
{
	public static class StatusToString
	{
		public static string Convert(Status status)
		{

			Dictionary<Status, string> dictionary = new Dictionary<Status, string>();

			dictionary.Add(Status.Online, UserStatusDictionary.Online);
			dictionary.Add(Status.Offline, UserStatusDictionary.Offline);
			dictionary.Add(Status.RightBack, UserStatusDictionary.RightBack);

			return dictionary[status];
		}
		
	}
	public enum Status
	{
		Online,
		RightBack,
		Offline
	}
}
