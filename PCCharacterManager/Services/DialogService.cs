using PCCharacterManager.DialogWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Services
{
	public class DialogService
	{
		private static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

		public static void RegisterDialog<TViewModel, TView>()
		{
			if (_mappings.ContainsKey(typeof(TViewModel)))
				return;

			_mappings.Add(typeof(TViewModel), typeof(TView));
		}

		public void ShowDialog<TViewModel>(Action<string> callBack)
		{
			var type = _mappings[typeof(TViewModel)];
			ShowDialogInternal(type, typeof(TViewModel), callBack);
		}

		public void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			var dialog = Activator.CreateInstance(_mappings[typeof(TViewModel)]) as Window;

			if (dialog is null)
				throw new InvalidCastException("Param window is not of type Window");

			dialog.DataContext = dataContext;

			EventHandler closeEventhandler = null;
			closeEventhandler = (s, e) =>
			{
				callBack(dialog.DialogResult.ToString());
				dialog.Closed -= closeEventhandler;
			};
			dialog.Closed += closeEventhandler;

			dialog.ShowDialog();
		}


		private static void ShowDialogInternal(Type window, Type dataContext, Action<string> callBack)
		{
			var dialog = Activator.CreateInstance(window) as Window;

			if (dialog is null)
				throw new InvalidCastException("Param window is not of type Window");

			dialog.DataContext = dataContext;

			EventHandler closeEventhandler = null;
			closeEventhandler = (s, e) =>
			{
				callBack(dialog.DialogResult.ToString());
				dialog.Closed -= closeEventhandler;
			};
			dialog.Closed += closeEventhandler;

			dialog.ShowDialog();
		}

		// https://www.youtube.com/watch?v=S8hEjLahNtU
		//private static void ShowDialogInternal(Type vType, Type vmType, Action<string> callBack)
		//{
		//	var dialog = new DialogWindow();

		//	EventHandler closeEventhandler = null;
		//	closeEventhandler = (s, e) =>
		//	{
		//		callBack(dialog.DialogResult.ToString());
		//		dialog.Closed -= closeEventhandler;
		//	};
		//	dialog.Closed += closeEventhandler;

		//	var content = Activator.CreateInstance(vType);

		//	if (vmType is not null)
		//	{
		//		var vm = Activator.CreateInstance(vmType);
		//		(content as FrameworkElement).DataContext = vm;
		//	}

		//	dialog.Content = content;

		//	dialog.ShowDialog();
		//}

	}
}
