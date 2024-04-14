using DesktopApplication.Config;
using DesktopApplication.Extensions;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
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
		private readonly MainFormConfigFactory _configFactory;

		public MainWindow()
		{
			InitializeComponent();

			_configFactory = new MainFormConfigFactory(this);
			_configFactory.RegisterService(new ClientService());
			_configFactory.RegisterService(new EmploeeService());
			_configFactory.RegisterService(new DeviceService());

			_configFactory.GetConfig<ClientDto>().Config();
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
			if (data is IDto dto)
			{
				dynamic config = _configFactory.GetConfig(dto.GetType());
				config.FillDataGridData();
			}
		}

		private async Task RemoveItem(dynamic item)
		{
			var dtoType = item.GetType();
			var service = _configFactory.GetConfig(dtoType).GetService();

			await service.DeleteAsync(item.Id);
		}

		private void DataGridEditRowButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			DataGridRow row = button.ParentOfType<DataGridRow>()!;
			var item = row.Item;

			var dtoType = item.GetType();
			var dialog = _configFactory.GetConfig(dtoType).CreateEditWindow(item);
			dialog.ShowDialog();
		}

		// TODO: change
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var tableTitle = this.dataGrid.FindName("tableTitle") as TextBlock;

			if (tableTitle!.Text.Equals(typeof(ClientDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = _configFactory.GetConfig<ClientDto>().CreateAddWindow(new ClientDto());
				dialog.ShowDialog();
			}
			else if (tableTitle!.Text.Equals(typeof(EmployeeDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = _configFactory.GetConfig<EmployeeDto>().CreateAddWindow(new EmployeeDto());
				dialog.ShowDialog();
			}
		}

		private void EmployeeButton_Click(object sender, RoutedEventArgs e)
		{
			var config = _configFactory.GetConfig<EmployeeDto>();
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
			var config = _configFactory.GetConfig<ClientDto>();
			config.Config();
		}

		private void DeviceButton_Click(object sender, RoutedEventArgs e)
		{
			var config = _configFactory.GetConfig<DeviceDto>();
			config.Config();
		}

		private void EHRButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ServiceWorkButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DevicePartProviderButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DevicePartDeliveryButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DevicePartButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ServiceButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}