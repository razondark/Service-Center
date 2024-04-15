using DesktopApplication.Extensions;
using MahApps.Metro.IconPacks;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Exceptions;
using ServiceCenterLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopApplication.Config
{
    public class MainFormConfig<T> where T : IDto
	{
		private readonly MainWindow _mainWindow;
		private readonly IDataService<T> _dataService;

		public MainFormConfig(MainWindow mainWindow, IDataService<T> dataService) 
		{
			_mainWindow = mainWindow;
			_dataService = dataService;
		}

		public IDataService<T> GetService()
		{
			return _dataService;
		}

		private Type? GetDataServiceDtoType()
		{
			// Get the interfaces that _dataService implements
			Type[] interfaces = _dataService.GetType().GetInterfaces();

			// Find the generic interface IDataService<T>
			Type dataServiceInterface = interfaces.FirstOrDefault(i =>
											i.IsGenericType &&
											i.GetGenericTypeDefinition() == typeof(IDataService<>));

			if (dataServiceInterface != null)
			{
				// Get the type argument T from the IDataService<T> interface
				return dataServiceInterface.GetGenericArguments().FirstOrDefault();
			}


			return null;
		}

		public async void Config(params (string, string)[] tabsText)
		{
			var dtoType = GetDataServiceDtoType();

			IEnumerable<object>? dtos = null;

			try
			{
				dtos = await _dataService.GetAllAsync() as IEnumerable<object>;
				FillDataGrid(dtos!, dtoType);
			}
			catch (ExceptionHandler)
			{
				FillDataGrid(null!, dtoType);
			}
			finally
			{
				var tableName = dtoType.GetCustomAttributes(typeof(DisplayNameAttribute), true)
								  .Cast<DisplayNameAttribute>()
								  .FirstOrDefault()?.DisplayName;
				SetTitle(tableName ?? "Unknown");
				SetTableTitle(tableName ?? "Unknown");
				SetTabsText(tabsText);
			}
		}

		private void ChangeTabButtonStyle(object sender)
		{
			var tabPanel = _mainWindow.dataGrid.FindName("tabsPanel") as StackPanel;
			if (tabPanel != null)
			{
				for (int i = 0; i < tabPanel.Children.Count; i++)
				{
					var currentButton = tabPanel.Children[i] as Button;
					if (currentButton != null)
					{
						if (currentButton == sender)
						{
							currentButton.BorderBrush = new SolidColorBrush(Color.FromRgb(0x78, 0x4f, 0xf2));
						}
						else
						{
							currentButton.BorderBrush = new SolidColorBrush(Color.FromRgb(0xda, 0xe2, 0xea));
						}
					}
				}
			}
		}

		public async Task<Window> CreateEditWindow(dynamic dto)
		{
			Type type = dto!.GetType();

			var elemsDictionary = new Dictionary<dynamic, PropertyInfo>();

			var editWindow = new Window
			{
				Title = "Редактирование",
				WindowStyle = WindowStyle.None,
				Style = (Style)Application.Current.Resources["dialogWindow"]
			};

			editWindow.MouseDown += (sender, e) =>
			{
				if (e.ChangedButton == MouseButton.Left)
				{
					editWindow.DragMove();
				}
			};

			var dtoId = await _dataService.GetByIdAsync(dto.Id);

			var stackPanel = new StackPanel();
			
			// add elems and data
			foreach (var property in type!.GetProperties())
			{
				var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
				var displayName = displayNameAttribute?.DisplayName ?? property.Name;

				var label = new Label
				{
					Content = displayName,
				};
				stackPanel.Children.Add(label);

				if (property.PropertyType == typeof(int))
				{
					if (property.Name.Equals("Id"))
					{
						var textBox = new TextBox
						{
							Text = property.GetValue(dto)!.ToString(),
							Width = 200,
							Height = 30,
							Margin = new Thickness(10),
							VerticalAlignment = VerticalAlignment.Center,
							HorizontalAlignment = HorizontalAlignment.Center
						};

						if (property.Name.Equals("Id"))
						{
							textBox.IsReadOnly = true;
							textBox.Background = Brushes.LightGray;
						}

						elemsDictionary.Add(textBox, property);
						stackPanel.Children.Add(textBox);
					}

					if (property.Name.Contains("Id") && property.Name.Length > 2)
					{
						IEnumerable<dynamic>? items = null;
						if (property.Name.Equals("IdClient"))
						{
							items = await new ClientService().GetAllAsync();
							items = items.OrderBy(i => i.Id);
							var comboBox = new ComboBox();
							foreach (var item in items)
							{
								comboBox.Items.Add(item.FullName);
							};
							comboBox.SelectedIndex = dtoId.IdClient - 1;

							elemsDictionary.Add(comboBox.SelectedItem, property);
							stackPanel.Children.Add(comboBox);
						}
						else if (property.Name.Equals("IdDevice"))
						{
							items = await new DeviceService().GetAllAsync();
							items = items.OrderBy(i => i.Id);
							var comboBox = new ComboBox();
							foreach (var item in items)
							{
								comboBox.Items.Add($"{item.Manufacturer} {item.Model}");
							}
							comboBox.SelectedIndex = dtoId.IdDevice - 1;

							elemsDictionary.Add(comboBox.SelectedItem, property);
							stackPanel.Children.Add(comboBox);
						}
						else if (property.Name.Equals("IdEmployee"))
						{
							items = await new EmploeeService().GetAllAsync();
							items = items.OrderBy(i => i.Id);
							var comboBox = new ComboBox();
							foreach (var item in items)
							{
								comboBox.Items.Add(item.FullName);
							}
							comboBox.SelectedIndex = dtoId.IdEmployee - 1;

							elemsDictionary.Add(comboBox.SelectedItem, property);
							stackPanel.Children.Add(comboBox);
						}
						else
						{
							throw new ArgumentException();
						}
					}
				}
				else if (property.PropertyType == typeof(string))
				{
					var textBox = new TextBox
					{
						Text = property.GetValue(dto)!.ToString(),
						Width = 200,
						Height = 30,
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					elemsDictionary.Add(textBox, property);
					stackPanel.Children.Add(textBox);
				}
				else if (property.PropertyType == typeof(decimal))
				{
					var value = (decimal)property.GetValue(dto)!;

					var textBox = new TextBox
					{
						Text = value.ToString("F"),
						Width = 200,
						Height = 30,
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					elemsDictionary.Add(textBox, property);
					stackPanel.Children.Add(textBox);
				}
				else if (property.PropertyType == typeof(DateTime) || (Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime)))
				{
					var datePicker = new DatePicker
					{
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center,
						DisplayDate = (DateTime)(property.GetValue(dto) ?? DateTime.Now), // Если значение null, устанавливаем текущую дату
						SelectedDate = (DateTime)(property.GetValue(dto) ?? DateTime.Now)
					};

					elemsDictionary.Add(datePicker, property);
					stackPanel.Children.Add(datePicker);
				}
				
			}

			var okButton = new Button
			{
				Content = "Изменить",
				Width = 60,
				Height = 30,
				Margin = new Thickness(10),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Right
			};

			okButton.Click += async (sender, e) =>
			{
				// update dto data
				foreach (var pair in elemsDictionary)
				{
					string newValue;
					if (pair.Key is int)
					{
						newValue = pair.Key.ToString();
					}
					else if (pair.Key is string)
					{
						newValue = pair.Key;
					}
					else
					{
						newValue = pair.Key.Text;
					}

					if (newValue == String.Empty)
					{
						MessageBox.Show("Все поля должны быть заполнены", "Ошибка");
						return;
					}

					if (pair.Value.PropertyType == typeof(int))
					{
						if (newValue is string)
						{
							// not working???
							var res = await _dataService.GetAllAsync();
							var id = res.Where(i => i.Id == dto.Id).ToList();
							pair.Value.SetValue(dto, Convert.ChangeType(Int32.Parse(newValue), pair.Value.PropertyType));
							continue;
						}
						pair.Value.SetValue(dto, Convert.ChangeType(Int32.Parse(newValue) - 1, pair.Value.PropertyType));
					}
					else if (pair.Value.PropertyType == typeof(DateTime) || (Nullable.GetUnderlyingType(pair.Value.PropertyType) == typeof(DateTime)))
					{
						if (Nullable.GetUnderlyingType(pair.Value.PropertyType) == typeof(DateTime))
						{
							pair.Value.SetValue(dto, Convert.ChangeType(DateTime.Parse(newValue), Nullable.GetUnderlyingType(pair.Value.PropertyType)));
						}
						else
						{
							pair.Value.SetValue(dto, Convert.ChangeType(DateTime.Parse(newValue), pair.Value.PropertyType));
						}
					}
					else
					{
						pair.Value.SetValue(dto, Convert.ChangeType(newValue, pair.Value.PropertyType));
					}
				}

				try
				{
					await _dataService.UpdateAsync(dto);
				}
				catch (ExceptionHandler ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}

				editWindow.Close();
				FillDataGridData();
			};

			var cancelButton = new Button
			{
				Content = "Отмена",
				Width = 60,
				Height = 30,
				Margin = new Thickness(10),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			cancelButton.Click += (sender, e) =>
			{
				editWindow.Close();
			};

			var panelButtons = new StackPanel
			{
				Orientation = Orientation.Horizontal
			};
			panelButtons.Children.Add(cancelButton);
			panelButtons.Children.Add(okButton);

			stackPanel.Children.Add(panelButtons);

			editWindow.Content = stackPanel;
			editWindow.Owner = _mainWindow;
			editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			return editWindow;
		}

		public async Task<Window> CreateAddWindow(dynamic dto)
		{
			Type type = dto!.GetType();
			var elemsDictionary = new Dictionary<dynamic, PropertyInfo>();

			var addWindow = new Window
			{
				Title = "Добавление",
				//Width = 600,
				//Height = 600,
				WindowStyle = WindowStyle.None,
				AllowsTransparency = true,
				Style = (Style)Application.Current.Resources["dialogWindow"]
			};

			addWindow.MouseDown += (sender, e) =>
			{
				if (e.ChangedButton == MouseButton.Left)
				{
					addWindow.DragMove();
				}
			};

			var stackPanel = new StackPanel();

			// add elems and data
			foreach (var property in type!.GetProperties())
			{
				var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
				var displayName = displayNameAttribute?.DisplayName ?? property.Name;

				if (property.Name.Equals("Id"))
				{
					continue;
				}

				var label = new Label
				{
					Content = displayName,
				};
				stackPanel.Children.Add(label);

				if (property.PropertyType == typeof(int))
				{
					if (property.Name.Contains("Id") && property.Name.Length > 2)
					{
						dynamic? items = null;
						if (property.Name.Equals("IdClient"))
						{
							items = await new ClientService().GetAllAsync();
							var comboBox = new ComboBox();
							foreach (var item in (IEnumerable<ClientDto>)items)
							{
								comboBox.Items.Add(item.FullName);
							}

							elemsDictionary.Add(comboBox, property);
							stackPanel.Children.Add(comboBox);
						}
						else if (property.Name.Equals("IdDevice"))
						{
							items = await new DeviceService().GetAllAsync();
							var comboBox = new ComboBox();
							foreach (var item in (IEnumerable<DeviceDto>)items)
							{
								comboBox.Items.Add($"{item.Manufacturer} {item.Model}");
							}

							elemsDictionary.Add(comboBox, property);
							stackPanel.Children.Add(comboBox);
						}
						else if (property.Name.Equals("IdEmployee"))
						{
							items = await  new EmploeeService().GetAllAsync();
							var comboBox = new ComboBox();
							foreach (var item in (IEnumerable<EmployeeDto>)items)
							{
								comboBox.Items.Add(item.FullName);
							}

							elemsDictionary.Add(comboBox, property);
							stackPanel.Children.Add(comboBox);
						}
						else
						{
							throw new ArgumentException();
						}
					}
				}
				else if (property.PropertyType == typeof(string))
				{
					var textBox = new TextBox
					{
						//Text = property.GetValue(dto)!.ToString(),
						Width = 200,
						Height = 30,
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					elemsDictionary.Add(textBox, property);
					stackPanel.Children.Add(textBox);
				}
				else if (property.PropertyType == typeof(decimal))
				{
					var value = (decimal)property.GetValue(dto)!;

					var textBox = new TextBox
					{
						//Text = value.ToString("F"),
						Width = 200,
						Height = 30,
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					elemsDictionary.Add(textBox, property);
					stackPanel.Children.Add(textBox);
				}
				else if (property.PropertyType == typeof(DateTime) || (Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime)))
				{
					var datePicker = new DatePicker
					{
						Margin = new Thickness(10),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Center,
						DisplayDate = (DateTime)(property.GetValue(dto) ?? DateTime.Now), // Если значение null, устанавливаем текущую дату
						SelectedDate = (DateTime)(property.GetValue(dto) ?? DateTime.Now)
					};

					elemsDictionary.Add(datePicker, property);
					stackPanel.Children.Add(datePicker);
				}

			}

			var okButton = new Button
			{
				Content = "Добавить",
				Width = 60,
				Height = 30,
				Margin = new Thickness(10),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Right,
			};

			okButton.Click += async (sender, e) =>
			{
				// update dto data
				foreach (var pair in elemsDictionary)
				{
					string newValue = pair.Key.Text;
					if (newValue == String.Empty)
					{
						MessageBox.Show("Все поля должны быть заполнены", "Ошибка");
						return;
					}

					pair.Value.SetValue(dto, Convert.ChangeType(newValue, pair.Value.PropertyType));
				}

				try
				{
					await _dataService.CreateAsync(dto);
				}
				catch (ExceptionHandler ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}

				addWindow.Close();
				FillDataGridData();
			};

			var cancelButton = new Button
			{
				Content = "Отмена",
				Width = 60,
				Height = 30,
				Margin = new Thickness(10),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			cancelButton.Click += (sender, e) =>
			{
				addWindow.Close();
			};


			var panelButtons = new StackPanel
			{
				Orientation = Orientation.Horizontal
			};
			panelButtons.Children.Add(cancelButton);
			panelButtons.Children.Add(okButton);

			stackPanel.Children.Add(panelButtons);

			addWindow.Content = stackPanel;
			addWindow.Owner = _mainWindow;
			addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			return addWindow;
		}

		private void TabButton_Click(object sender, RoutedEventArgs e)
		{
			ChangeTabButtonStyle(sender);

			// TODO: add tab buttons
		}

		// (string, string) -> (english name, russian text)
		private void SetTabsText((string, string)[] tabsText)
		{
			var tabPanel = _mainWindow.dataGrid.FindName("tabsPanel") as StackPanel;

			for (int i = 0; i < tabsText.Length; i++)
			{
				var button = new Button()
				{
					Content = tabsText[i].Item2, // russian text
					Style = (Style)Application.Current.Resources["tabButton"],
					Name = tabsText[i].Item1 // english name
				};

				if (i == 0)
				{
					button.BorderBrush = new SolidColorBrush(Color.FromRgb(0x78, 0x4f, 0xf2));
				}

				button.Click += TabButton_Click;

				tabPanel!.Children.Add(button);
			}
		}

		private void SetTableTitle(string tableText)
		{
			var title = _mainWindow.dataGrid.FindName("tableTitle") as TextBlock;
			title!.Text = tableText;
		}

		private void SetTitle(string titleText)
		{
			var title = _mainWindow.dataGrid.FindName("pageTitle") as TextBlock;
			title!.Text = titleText;
		}

		public async void FillDataGridData()
		{
			try
			{
				var data = await _dataService.GetAllAsync();
				FillDataGrid((IEnumerable<object>)data!);
			}
			catch (ExceptionHandler ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private DataGridRow GetClickedRow(MouseButtonEventArgs e)
		{
			DependencyObject dep = (DependencyObject)e.OriginalSource;

			// Ищем родительский элемент типа DataGridRow
			while (dep != null && !(dep is DataGridRow))
			{
				dep = VisualTreeHelper.GetParent(dep);
			}

			return dep as DataGridRow;
		}

		private void FillDataGrid(IEnumerable<object> elements, Type? elementsType = null)
		{
			var buttons = _mainWindow.dataGrid.FindName("actionButtons") as DataGridTemplateColumn;
			_mainWindow.dataGrid.Columns.Clear();

			// TODO: error _dataservice???? always clientDto type in T
			//_mainWindow.dataGrid.RemoveHandler(_mainWindow.dataGrid.MouseLeftButtonDown, null);
			//_mainWindow.dataGrid.MouseDoubleClick += async (sender, e) =>
			//{
			//	DataGridRow row = GetClickedRow(e);
			//	var item = row.Item;

			//	await _dataService.GetByIdAsync(item.Id);

			//	var i = 0;
			//};

			Type? elementType = null;
			if (elementsType is not null)
			{
				elementType = elementsType;
			}

			if (elementsType is null)
			{
				var firstElement = elements.FirstOrDefault();
				if (firstElement == null)
					return;

				elementType = firstElement.GetType();
			}

			foreach (var property in elementType.GetProperties())
			{
				var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
				if (displayNameAttribute is null || (property.Name.Contains("Id") && property.Name.Length > 2))
				{
					continue;
				}
				var displayName = displayNameAttribute?.DisplayName ?? property.Name;

				var column = new DataGridTextColumn
				{
					IsReadOnly = true,
					Header = displayName,
					Binding = new Binding(property.Name),
					Width = new DataGridLength(1, DataGridLengthUnitType.Star),
				};

				if (property.Name.Equals("Id"))
				{
					column.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
				}

				_mainWindow.dataGrid.Columns.Add(column);
			}

			_mainWindow.dataGrid.Columns.Add(AddDataGridRowsButtons(buttons!));

			if (elements is not null)
			{
				elements = elements.OrderBy(e => (int)e.GetType().GetProperty("Id")!.GetValue(e)!);

				var collection = new ObservableCollection<object>(elements);
				_mainWindow.dataGrid.ItemsSource = collection;
			}
		}

		private void AddDataGridCheckBoxColumn()
		{

		}

		private DataGridTemplateColumn? AddDataGridRowsButtons(DataGridTemplateColumn actionButtonsColumn)
		{
			if (actionButtonsColumn != null)
			{
				DataTemplate cellTemplate = actionButtonsColumn.CellTemplate;
				StackPanel? stackPanel = cellTemplate.LoadContent() as StackPanel;

				if (stackPanel != null)
				{
					stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
				}
			}

			return actionButtonsColumn;
		}
	}
}
