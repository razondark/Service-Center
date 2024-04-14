using ServiceCenterLibrary.Dto;
using ServiceCenterLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Config
{
	public class MainFormConfigFactory
	{
		private readonly MainWindow _mainWindow;
		private readonly Dictionary<Type, object> _configInstances = new Dictionary<Type, object>();
		private readonly Dictionary<Type, object> _serviceInstances = new Dictionary<Type, object>();

		public MainFormConfigFactory(MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
		}

		public void RegisterService<T>(IDataService<T> service)
		{
			_serviceInstances[typeof(T)] = service;
		}

		public MainFormConfig<T> GetConfig<T>() where T : IDto
		{
			Type dtoType = typeof(T);
			if (!_configInstances.TryGetValue(dtoType, out object? config))
			{
				if (_serviceInstances.TryGetValue(dtoType, out object? service))
				{
					config = new MainFormConfig<T>(_mainWindow, (IDataService<T>)service);
					_configInstances[dtoType] = config;
				}
				else
				{
					throw new InvalidOperationException($"No service registered for type {dtoType.Name}");
				}
			}
			return (MainFormConfig<T>)config;
		}

		public dynamic GetConfig(Type dtoType)
		{
			if (!_configInstances.TryGetValue(dtoType, out object? config))
			{
				Type dataServiceType = typeof(IDataService<>).MakeGenericType(dtoType);
				dynamic dataService = Activator.CreateInstance(dataServiceType)!;


				Type mainFormConfigType = typeof(MainFormConfig<>).MakeGenericType(dtoType);
				config = Activator.CreateInstance(mainFormConfigType, _mainWindow, dataService);

				_configInstances[dtoType] = config;
			}
			return config;
		}
	}
}
