using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DesktopApplication.Extensions
{
	public static class DataGridExtension
	{
		public static T? ParentOfType<T>(this DependencyObject element) where T : DependencyObject
		{
			var parent = VisualTreeHelper.GetParent(element);

			while (parent != null)
			{
				if (parent is T)
					return (T)parent;

				parent = VisualTreeHelper.GetParent(parent);
			}

			return null;
		}
	}
}
