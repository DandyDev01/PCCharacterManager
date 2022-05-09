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
		public Action<ListViewSelectableItemViewModel> OnSelect { get; set; }
		public Action<ListViewSelectableItemViewModel> OnDeselect { get; set; }

		private string boundItem;
		public string BoundItem
		{
			get { return boundItem; }
			set { OnPropertyChaged(ref boundItem, value); }
		}

		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				OnPropertyChaged(ref isSelected, !isSelected);
				if (isSelected)
					OnSelect?.Invoke(this);
				else
					OnDeselect?.Invoke(this);
			}
		}

		public ListViewSelectableItemViewModel(string item)
		{
			boundItem = item;
		}
	}
}
