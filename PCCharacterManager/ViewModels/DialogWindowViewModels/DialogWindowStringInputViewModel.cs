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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="window">the dialog window that opens</param>
		public DialogWindowStringInputViewModel()
		{
			_answer = string.Empty;
			_message = string.Empty;
		}

		public DialogWindowStringInputViewModel(string message)
		{
			_answer = string.Empty;
			_message = message;
		}
	} // end class
}
