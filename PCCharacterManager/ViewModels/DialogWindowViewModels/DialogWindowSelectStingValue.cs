﻿using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowSelectStingValue : ObservableObject
	{
		private readonly Window dialogWindow;
		private readonly ListViewMultiSelectItemsLimitedCountViewModel limitedMultiSelectVM;

		public ListViewMultiSelectItemsLimitedCountViewModel LimitedMultiSelectVM { get { return limitedMultiSelectVM; } }
		public IEnumerable<string> SelectedItems { get { return LimitedMultiSelectVM.SelectedItems; } }

		public ICommand AddSelectedCommand { get; set; }
		public ICommand CloseCommand { get; set; }

		public DialogWindowSelectStingValue(Window _dialogWindow, string[] options, int maxSelections = 1)
		{
			dialogWindow = _dialogWindow;
			limitedMultiSelectVM = new ListViewMultiSelectItemsLimitedCountViewModel(maxSelections, options.ToList());

			AddSelectedCommand = new RelayCommand(AddSelected);
			CloseCommand = new RelayCommand(Close);

		}

		/// <summary>
		/// Add the selected to the in param _selected
		/// </summary>
		private void AddSelected()
		{
			dialogWindow.DialogResult = true;
			dialogWindow.Close();
		}

		/// <summary>
		/// Will close the dialog window
		/// </summary>
		private void Close()
		{
			dialogWindow.Close();
		}
	}
}