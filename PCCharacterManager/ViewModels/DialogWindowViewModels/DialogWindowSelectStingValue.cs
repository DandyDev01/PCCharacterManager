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
	public class DialogWindowSelectStingValue : ObservableObject
	{
		private readonly Window _window;
		private readonly ListViewMultiSelectItemsLimitedCountViewModel _limitedMultiSelectVM;

		public ListViewMultiSelectItemsLimitedCountViewModel LimitedMultiSelectVM { get { return _limitedMultiSelectVM; } }
		public IEnumerable<string> SelectedItems { get { return LimitedMultiSelectVM.SelectedItems; } }

		public ICommand AddSelectedCommand { get; set; }
		public ICommand CloseCommand { get; set; }

		public DialogWindowSelectStingValue(Window window, string[] options, int maxSelections = 1)
		{
			_window = window;
			_limitedMultiSelectVM = new ListViewMultiSelectItemsLimitedCountViewModel(maxSelections, options.ToList());

			AddSelectedCommand = new RelayCommand(AddSelected);
			CloseCommand = new RelayCommand(Close);

		}

		/// <summary>
		/// Add the selected to the in param _selected
		/// </summary>
		private void AddSelected()
		{
			_window.DialogResult = true;
			_window.Close();
		}

		/// <summary>
		/// Will close the dialog window
		/// </summary>
		private void Close()
		{
			_window.Close();
		}
	}
}
