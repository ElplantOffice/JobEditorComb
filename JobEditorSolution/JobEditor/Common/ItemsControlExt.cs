using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace JobEditor.Common
{
	public static class ItemsControlExt
	{
		public static void ScrollIntoView(this ItemsControl control, object item)
		{
			FrameworkElement frameworkElement = control.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			frameworkElement.BringIntoView();
		}

		public static void ScrollIntoView(this ItemsControl control)
		{
			int count = control.Items.Count;
			if (count == 0)
			{
				return;
			}
			object item = control.Items[count - 1];
			control.ScrollIntoView(item);
		}
	}
}