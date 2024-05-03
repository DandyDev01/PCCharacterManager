using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class ListViewSelectableItemViewModel : ObservableObject
	{
		public Action<ListViewSelectableItemViewModel>? OnSelect { get; set; }
		public Action<ListViewSelectableItemViewModel>? OnDeselect { get; set; }

		private string _boundItem;
		public string BoundItem
		{
			get { return _boundItem; }
			set { OnPropertyChanged(ref _boundItem, value); }
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				OnPropertyChanged(ref _isSelected, !_isSelected);
				if (_isSelected)
					OnSelect?.Invoke(this);
				else
					OnDeselect?.Invoke(this);
			}
		}

		public ListViewSelectableItemViewModel(string item)
		{
			_boundItem = item;
		}
	}
}
