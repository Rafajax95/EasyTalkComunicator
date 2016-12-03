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
using EasyTalk.Utils.Dictionaries;
using EasyTalk.Utils.Models;
using static EasyTalk.Utils.StatusToString;
using EasyTalk.Utils;
using EasyTalk.Client;
using EasyTalk.ClientUI.Views;

namespace EasyTalk.ClientUI.UserControls
{
	/// <summary>
	/// Interaction logic for UserLabel.xaml
	/// </summary>
	public partial class UserLabel : UserControl
	{
		public User User;
		WaitingRoomView parent;
		public UserLabel(User user, WaitingRoomView parent)
		{
			InitializeComponent();
			UpdateContent(user);
			this.parent = parent;
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (User.Status == Status.Online)
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
			else
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF2FF6F"));
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			if (User.Status == Status.Online)
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
			else
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDE200"));
		}

		internal void UpdateContent(User user)
		{
			this.User = user;
			this.User = user;

			UserNameLB.Content = User.Name;
			UserStatusLB.Content = StatusToString.Convert(User.Status);
			if (User.Status == Status.Online)
			{
				if (this.IsMouseOver)
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF225305"));
				}

				else
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF225305"));
				}

			}
			else
			{
				if (this.IsMouseOver)
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF2FF6F"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
				}
				else
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDE200"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
				}

			}
		}


		private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			parent.Connection.ActualRecipient = User;

			if (parent.childRoom == null)
				parent.MessageRecipientLB.Content = User.Name;
			else
				parent.childRoom.MessageRecipientLB.Content = User.Name;

		}
	}
}
