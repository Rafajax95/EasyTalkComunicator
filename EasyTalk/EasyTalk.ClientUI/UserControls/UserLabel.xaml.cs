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

namespace EasyTalk.ClientUI.UserControls
{
	/// <summary>
	/// Interaction logic for UserLabel.xaml
	/// </summary>
	public partial class UserLabel : UserControl
	{
		public User User;
		Connection connection;
		Label recipientLabel;
		public UserLabel(User user, Connection connection, Label recipientLabel)
		{
			InitializeComponent();
			UpdateContent(user);
			this.connection = connection;
			this.recipientLabel = recipientLabel;
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (User.Status == Status.Online)
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
			else
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBCCD15"));
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
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF58CD15"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF225305"));
				}

				else
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF225305"));
				}

			}
			else
			{
				if (this.IsMouseOver)
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBCCD15"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA8CB0C"));
				}
				else
				{
					Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB1D650"));
					Content.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA8CB0C"));
				}

			}
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			if (User.Status == Status.Online)
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D650"));
			else
				Content.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB1D650"));
		}

		private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			connection.ActualRecipient = User;
			recipientLabel.Content = User.Name;
		}
	}
}
