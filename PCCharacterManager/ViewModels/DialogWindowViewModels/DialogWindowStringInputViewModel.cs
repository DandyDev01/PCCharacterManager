using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowStringInputViewModel : ObservableObject
	{
		private readonly Window _window;

		private string _answer;
		public string Answer
		{
			get { return _answer; }
			set { OnPropertyChanged(ref _answer, value); }
		}

		private string _message;
		public string Message
		{
			get { return _message; }
		}

		public ICommand OkCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="window">the dialog window that opens</param>
		public DialogWindowStringInputViewModel(Window window)
		{
			_answer = string.Empty;
			_message = string.Empty;

			_window = window;
			OkCommand = new RelayCommand(Ok);
			CloseCommand = new RelayCommand(Close);
		}

		public DialogWindowStringInputViewModel(Window window, string message)
		{
			_answer = string.Empty;
			_message = message;

			_window = window;
			OkCommand = new RelayCommand(Ok);
			CloseCommand = new RelayCommand(Close);
		}

		/// <summary>
		/// Will add the selected characters to the encounter 
		/// </summary>
		private void Ok()
		{
			_window.DialogResult = true;
			_window.Close();
		}

		/// <summary>
		/// Closes the dialog window
		/// </summary>
		private void Close()
		{
			Answer = string.Empty;
			_window.DialogResult = false;
			_window.Close();
		}

	} // end class
}
