using DesktopApplication.Config;
using DesktopApplication.Extensions;
using DesktopApplication.Forms;
using Service_Center_Backend.Models;
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

		private AccountDto _accountDto;

		private RenderTargetBitmap CreateLogo(string name)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				// Задаем цвет фона
				drawingContext.DrawRectangle(
					new SolidColorBrush(Colors.White), // Цвет фона
					null, // Стиль обводки
					new Rect(0, 0, 80, 80)); // Размеры прямоугольника

				// Создаем форму для буквы
				EllipseGeometry ellipseGeometry = new EllipseGeometry(new Rect(0, 0, 80, 80));

				// Задаем цвет и форму для буквы
				drawingContext.DrawGeometry(
					new SolidColorBrush(Colors.Blue), // Цвет буквы
					new Pen(Brushes.Black, 1), // Стиль обводки буквы
					ellipseGeometry); // Форма буквы

				// Рисуем букву
				FormattedText formattedText = new FormattedText(
					name, // Буква
					System.Globalization.CultureInfo.GetCultureInfo("en-us"), // Культура
					FlowDirection.LeftToRight, // Направление текста
					new Typeface("Arial"), // Шрифт
					48, // Размер шрифта
					Brushes.White); // Цвет текста

				// Вычисляем позицию текста внутри эллипса
				double textWidth = formattedText.Width;
				double textHeight = formattedText.Height;
				double x = (80 - textWidth) / 2;
				double y = (80 - textHeight) / 2;

				// Рисуем текст
				drawingContext.DrawText(formattedText, new Point(x, y));
			}

			// Преобразуем DrawingVisual в BitmapSource
			RenderTargetBitmap bitmap = new RenderTargetBitmap(
				80, // Ширина
				80, // Высота
				96, // Горизонтальное разрешение
				96, // Вертикальное разрешение
				PixelFormats.Pbgra32); // Формат пикселей
			bitmap.Render(drawingVisual);

			// Устанавливаем BitmapSource в качестве источника для ImageBrush
			return bitmap;
		}

		public MainWindow()
		{
			InitializeComponent();

			_configFactory = new MainFormConfigFactory(this);
			_configFactory.RegisterService(new ClientService());
			_configFactory.RegisterService(new EmploeeService());
			_configFactory.RegisterService(new DeviceService());
			_configFactory.RegisterService(new EquipmentHandoverReceiptService());

			_configFactory.GetConfig<ClientDto>().Config();

			this.dataGrid.SelectionMode = DataGridSelectionMode.Extended;
			GetLoginForm();
		}

		private void GetLoginForm()
		{
			this.Visibility = Visibility.Hidden;
			var loginForm = new LoginForm();
			var result = loginForm.ShowDialog();

			if (result == true)
			{
				_accountDto = loginForm.Account;
			}
			else
			{
				Application.Current.Shutdown();
			}

			this.username.Text = _accountDto.Login;
			this.userIcon.ImageSource = CreateLogo(_accountDto.Login.Substring(0, 1).ToUpper().ToString());

			// start menu buttons
			this.clientsButton.Visibility = Visibility.Visible;
			this.employeesButton.Visibility = Visibility.Visible;
			this.devicesButton.Visibility = Visibility.Visible;
			this.ehrButton.Visibility = Visibility.Visible;
			this.serviceButton.Visibility = Visibility.Visible;
			this.providerButton.Visibility = Visibility.Visible;
			this.devicePartDeliveryButton.Visibility = Visibility.Visible;
			this.devicePartButton.Visibility = Visibility.Visible;
			this.serviceWorkButton.Visibility = Visibility.Visible;

			this.Visibility = Visibility.Visible;

			if (_accountDto!.Status.Equals(AccountRoles.Admin))
			{
				//buttonRemove.Visibility = Visibility.Visible;

				this.dbButton.Visibility = Visibility.Visible;
			}
			else if (_accountDto!.Status.Equals(AccountRoles.Сashier))
			{
				// склад поставки поставщики
				this.providerButton.Visibility = Visibility.Collapsed;
				this.devicePartButton.Visibility = Visibility.Collapsed;
				this.devicePartDeliveryButton.Visibility = Visibility.Collapsed;

				this.dbButton.Visibility = Visibility.Collapsed;
			}
			else if (_accountDto!.Status.Equals(AccountRoles.Repairman))
			{
				this.clientsButton.Visibility = Visibility.Collapsed;
				this.employeesButton.Visibility = Visibility.Collapsed;
				this.providerButton.Visibility = Visibility.Collapsed;

				this.dbButton.Visibility = Visibility.Collapsed;
			}
			else if (_accountDto!.Status.Equals(AccountRoles.Manager))
			{

				this.dbButton.Visibility = Visibility.Collapsed;
			}
			else if (_accountDto!.Status.Equals(AccountRoles.Director))
			{
				this.dbButton.Visibility = Visibility.Collapsed;
			}
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				try
				{
					this.DragMove();
				}
				catch { }
			}
		}

		private bool IsMaximized = false;
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 3)
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

		private async void DataGridEditRowButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			DataGridRow row = button.ParentOfType<DataGridRow>()!;
			var item = row.Item;

			var dtoType = item.GetType();
			var dialog = await _configFactory.GetConfig(dtoType).CreateEditWindow(item);
			dialog.ShowDialog();
		}

		// TODO: change
		private async void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var tableTitle = this.dataGrid.FindName("tableTitle") as TextBlock;

			if (tableTitle!.Text.Equals(typeof(ClientDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = await _configFactory.GetConfig<ClientDto>().CreateAddWindow(new ClientDto());
				dialog.ShowDialog();
			}
			else if (tableTitle!.Text.Equals(typeof(EmployeeDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = await _configFactory.GetConfig<EmployeeDto>().CreateAddWindow(new EmployeeDto());
				dialog.ShowDialog();
			}
			else if (tableTitle!.Text.Equals(typeof(DeviceDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = await _configFactory.GetConfig<DeviceDto>().CreateAddWindow(new DeviceDto());
				dialog.ShowDialog();
			}
			else if (tableTitle!.Text.Equals(typeof(EquipmentHandoverReceiptDto).GetCustomAttributes(typeof(DisplayNameAttribute), true)
														  .Cast<DisplayNameAttribute>()
														  .FirstOrDefault()?.DisplayName))
			{
				var dialog = await _configFactory.GetConfig<EquipmentHandoverReceiptDto>().CreateAddWindow(new EquipmentHandoverReceiptDto());
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
			var config = _configFactory.GetConfig<EquipmentHandoverReceiptDto>();
			config.Config();
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

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			GetLoginForm();
		}

		private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.dataGrid.SelectedItems.Clear();
			
			var searchText = this.txtSearch.Text;
			var items = dataGrid.ItemsSource;

			if (items is not null && this.dataGrid.HasItems && !string.IsNullOrEmpty(searchText))
			{
				if (items.Cast<object>().First() is not IDto)
				{
					return;
				}

				foreach (var item in items)
				{
					foreach (var property in item.GetType().GetProperties())
					{
						var value = property.GetValue(item, null).ToString();

						if (value.Contains(searchText, StringComparison.OrdinalIgnoreCase))
						{
							// Выделяем строку, если найдено совпадение
							dataGrid.SelectedItems.Add(item);
						}
					}
				}
			}
		}
	}
}