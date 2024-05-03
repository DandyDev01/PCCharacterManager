using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
    class DialogWindowChangeHealthViewModel : ObservableObject
    {
		private readonly Window _window;

		public int Amount { get; private set; }

		private string _answer;
		public string Answer
		{
			get
			{
				return _answer;
			}
			set
			{
				OnPropertyChanged(ref _answer, value);
			}
		}

		private bool _isTempHealth;
		public bool IsTempHealth
		{
			get
			{
				return _isTempHealth;
			}
			set
			{
				OnPropertyChanged(ref _isTempHealth, value);
			}
		}

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowChangeHealthViewModel(Window window)
		{
			_window = window;
			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
			_answer = string.Empty;
		}

		private void Cancel()
		{
			_window.DialogResult = false;
			_window.Close();
		}

		private void Ok()
		{
			try
			{
				Amount = int.Parse(_answer);
			}
			catch
			{
				MessageBox.Show("value must be a whole number", "invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_window.DialogResult = true;
			_window.Close();
		}
	}
}
