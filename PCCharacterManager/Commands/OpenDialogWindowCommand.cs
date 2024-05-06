using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class OpenDialogWindowCommand<T> : BaseCommand
	{
		private Window _window;
		private T _windowDataContext;

		public OpenDialogWindowCommand(Window window, T windowDataContext)
		{
			_window = window;
			_windowDataContext = windowDataContext;
		}

		public override void Execute(object? parameter)
		{
			_window.DataContext = _windowDataContext;
			_window.ShowDialog();
		}
	}
}
