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
	internal class DialogWindowListViewSelectItemViewModel : ObservableObject
	{
		private readonly ListViewMultiSelectItemsLimitedCountViewModel _limitedMultiSelectVM;

		public ListViewMultiSelectItemsLimitedCountViewModel LimitedMultiSelectVM { get { return _limitedMultiSelectVM; } }
		public IEnumerable<string> SelectedItems { get { return LimitedMultiSelectVM.SelectedItems; } }

		public DialogWindowListViewSelectItemViewModel(string[] options, int maxSelections = 1)
		{
			_limitedMultiSelectVM = new ListViewMultiSelectItemsLimitedCountViewModel(maxSelections, options.ToList());
			OnPropertyChanged("MonsterNPCsToDisplay");
		}
	}
}
