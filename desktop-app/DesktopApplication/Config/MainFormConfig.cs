using MahApps.Metro.IconPacks;
using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Dto.Response;
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
using System.Windows.Media;

namespace DesktopApplication.Config
{
    public class MainFormConfig<T>
	{
		private readonly MainWindow _mainWindow;
		private readonly IDataService<T> _dataService;

		public MainFormConfig(MainWindow mainWindow, IDataService<T> dataService) 
		{
			_mainWindow = mainWindow;
			_dataService = dataService;
		}

		public async void Config(params (string, string)[] tabsText)
		{
			var dtos = (IEnumerable<object>)await _dataService.GetAllAsync();
			var tableName = dtos!.FirstOrDefault()?.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), true)
								  .Cast<DisplayNameAttribute>()
								  .FirstOrDefault()?.DisplayName;

			FillDataGrid(dtos);
			SetTitle(tableName ?? "Unknown");
			SetTableTitle(tableName ?? "Unknown");
			SetTabsText(tabsText);
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
			this.FillDataGrid((IEnumerable<object>)await _dataService.GetAllAsync());
		}

		private void FillDataGrid(IEnumerable<object> elements)
		{
			var buttons = _mainWindow.dataGrid.FindName("actionButtons") as DataGridTemplateColumn;
			_mainWindow.dataGrid.Columns.Clear();

			// Получаем первый элемент коллекции, чтобы определить его тип
			var firstElement = elements.FirstOrDefault();
			if (firstElement == null)
				return;

			var elementType = firstElement.GetType();

			foreach (var property in elementType.GetProperties())
			{
				var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
				var displayName = displayNameAttribute?.DisplayName ?? property.Name;

				var column = new DataGridTextColumn
				{
					IsReadOnly = true,
					Header = displayName,
					Binding = new Binding(property.Name),
					Width = new DataGridLength(1, DataGridLengthUnitType.Star)
				};

				if (property.Name == "Id")
				{
					column.Width = new DataGridLength(0.5, DataGridLengthUnitType.Star);
				}

				_mainWindow.dataGrid.Columns.Add(column);
			}

			_mainWindow.dataGrid.Columns.Add(AddDataGridRowsButtons(buttons));

			var collection = new ObservableCollection<object>(elements);
			_mainWindow.dataGrid.ItemsSource = collection;
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
