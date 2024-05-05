using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Condition = PCCharacterManager.Models.Condition;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	class DialogWindowAddConditionViewModel : ObservableObject
	{
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				OnPropertyChanged(ref _description, value);
			}
		}

		private int _duration;
		public int Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				OnPropertyChanged(ref _duration, value);
			}
		}

		private readonly Window _window;
		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowAddConditionViewModel(Window window)
		{
			_window = window;
			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
			_name = string.Empty;
			_description = string.Empty;
			
		}

		private void Cancel()
		{
			_window.DialogResult = false;
			_window.Close();
		}

		private void Ok()
		{
			_window.DialogResult = true;
			_window.Close();
		}
	}
}
