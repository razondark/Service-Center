﻿using DesktopApplication.Config;
using DesktopApplication.Extensions;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DesktopApplication
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var config = new MainFormConfig<ClientDto>(this, new ClientService());
			config.Config();
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}

		private bool IsMaximized = false;
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				if (IsMaximized) 
				{
					this.WindowState = WindowState.Normal;
					this.Width = 1280;
					this.Height = 720;

					IsMaximized = false;
				}
				else
				{
					this.WindowState = WindowState.Maximized;

					IsMaximized = true;
				}
			}
		}

		private void DataGridEditRowButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			DataGridRow row = button.ParentOfType<DataGridRow>()!;
			var item = row.Item;

			// TODO: add edit form

			var i = 0;
		}

		private async void DataGridRemoveRowButton_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				DataGridRow row = (sender as Button)!.ParentOfType<DataGridRow>()!;
				var item = row.Item;

				await RemoveItem(item);
				UpdateDataGridData(item);
			}
		}

		private void UpdateDataGridData(object data)
		{
			if (data is ClientDto)
			{
				var config = new MainFormConfig<ClientDto>(this, new ClientService());
				config.FillDataGridData();
			}
			else if (data is EmployeeDto)
			{
				var config = new MainFormConfig<EmployeeDto>(this, new EmploeeService());
				config.FillDataGridData();
			}
		}

		private async Task RemoveItem(object item)
		{
			if (item is ClientDto clientDto)
			{
				var clientService = new ClientService();
				await clientService.DeleteAsync(clientDto.Id);
			}
			else if (item is EmployeeDto employeeDto)
			{
				var employeeService = new EmploeeService();
				await employeeService.DeleteAsync(employeeDto.Id);
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void EmployeeButton_Click(object sender, RoutedEventArgs e)
		{
			var config = new MainFormConfig<EmployeeDto>(this, new EmploeeService());
			//config.Config(
			//	("allEmployees", "Все сотрудники"), 
			//	("masters", "Мастеры"), 
			//	("cashiers", "Кассиры"), 
			//	("managers", "Менеджеры")
			//);
			config.Config();
		}

		private void ClientsButton_Click(object sender, RoutedEventArgs e)
		{
			var config = new MainFormConfig<ClientDto>(this, new ClientService());
			config.Config();
		}

		private void DeviceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void EHRButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ServiceWorkButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}