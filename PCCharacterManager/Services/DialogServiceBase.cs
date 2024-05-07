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
	public abstract class DialogServiceBase
	{
		protected static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

		/// <summary>
		/// Registering is done in the MainWindow.xaml.cs file
		/// </summary>
		/// <typeparam name="TViewModel"></typeparam>
		/// <typeparam name="TView"></typeparam>
		public static void RegisterDialog<TViewModel, TView>()
		{
			if (_mappings.ContainsKey(typeof(TViewModel)))
				return;

			_mappings.Add(typeof(TViewModel), typeof(TView));
		}

		public abstract void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack);
	}

	// https://www.youtube.com/watch?v=S8hEjLahNtU
	public class DialogService : DialogServiceBase
	{
		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			var dialogWindow = _mappings[typeof(TViewModel)];

			if (dialogWindow == null)
				throw new Exception("Dialog window is not mapped");

			var dialog = Activator.CreateInstance(dialogWindow) as Window;

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
	}
}
