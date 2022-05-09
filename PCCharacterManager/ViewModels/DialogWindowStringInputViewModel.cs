using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class StringInputDialogWindowViewModel : ObservableObject
	{
		private readonly Window dialogWindowl;

		private string answer;
		public string Answer
		{
			get { return answer; }
			set { OnPropertyChaged(ref answer, value); }
		}

		private string message;
		public string Message
		{
			get { return message; }
		}

		public ICommand OkCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_dialogWindow">the dialog window that opens</param>
		public StringInputDialogWindowViewModel(Window _dialogWindow)
		{
			answer = string.Empty;
			message = string.Empty;

			dialogWindowl = _dialogWindow;
			OkCommand = new RelayCommand(Ok);
			CloseCommand = new RelayCommand(Close);
		}

		public StringInputDialogWindowViewModel(Window _dialogWindow, string _message)
		{
			answer = string.Empty;
			message = _message;

			dialogWindowl = _dialogWindow;
			OkCommand = new RelayCommand(Ok);
			CloseCommand = new RelayCommand(Close);
		}

		/// <summary>
		/// Will add the selected characters to the encounter 
		/// </summary>
		private void Ok()
		{
			dialogWindowl.DialogResult = true;
			dialogWindowl.Close();
		}

		/// <summary>
		/// Closes the dialog window
		/// </summary>
		private void Close()
		{
			Answer = string.Empty;
			dialogWindowl.DialogResult = false;
			dialogWindowl.Close();
		}

	} // end class
}
