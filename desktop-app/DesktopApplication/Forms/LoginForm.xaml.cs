using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Exceptions;
using ServiceCenterLibrary.Services;
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
using System.Windows.Shapes;

namespace DesktopApplication.Forms
{
	/// <summary>
	/// Логика взаимодействия для LoginForm.xaml
	/// </summary>
	public partial class LoginForm : Window
	{
		public LoginForm()
		{
			InitializeComponent();
		}

		public AccountDto Account { get; set; }

		private AccountService _accountService = new AccountService();

		private async void buttonLogin_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Account = await _accountService.LoginAsync(loginTextBox.Text, passwordTextBox.Text);

				this.DialogResult = true;
				this.Close();
			}
			catch (ExceptionHandler ex)
			{
				MessageBox.Show(ex.Message, "Ошибка");
			}
		}

		private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (loginTextBox.Text.Equals("login"))
			{
				loginTextBox.Text = String.Empty;
			}
		}

		private void passwordTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (passwordTextBox.Text.Equals("password"))
			{
				passwordTextBox.Text = String.Empty;
			}
		}
	}
}
