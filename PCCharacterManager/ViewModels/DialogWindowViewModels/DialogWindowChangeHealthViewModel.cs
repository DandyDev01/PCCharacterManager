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
		private readonly Window window;

		public int Amount { get; private set; }

		private string answer;
		public string Answer
		{
			get
			{
				return answer;
			}
			set
			{
				OnPropertyChanged(ref answer, value);
			}
		}

		private bool isTempHealth;
		public bool IsTempHealth
		{
			get
			{
				return isTempHealth;
			}
			set
			{
				OnPropertyChanged(ref isTempHealth, value);
			}
		}

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowChangeHealthViewModel(Window _window)
		{
			window = _window;
			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
		}

		private void Cancel()
		{
			window.DialogResult = false;
			window.Close();
		}

		private void Ok()
		{
			try
			{
				Amount = int.Parse(answer);
			}
			catch (Exception ex)
			{
				MessageBox.Show("value must be a whole number", "invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			window.DialogResult = true;
			window.Close();
		}
	}
}
