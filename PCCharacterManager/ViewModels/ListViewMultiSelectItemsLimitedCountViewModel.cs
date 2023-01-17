using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	// NOTE: Use ChooseItemListViewModel from CharacterCreator as refrence
	public class ListViewMultiSelectItemsLimitedCountViewModel : ObservableObject
	{
		private List<string> possibleOptions;
		private List<string> selectedItems;
		private int amountToBeSelected;
		private string listSelectedItem;

		public int AmountSelected { get; private set; }
		public int AmountToBeSelected
		{
			get { return amountToBeSelected; }
			set { OnPropertyChanged(ref amountToBeSelected, value); }
		}

		/// <summary>
		/// Item that is selected in the list
		/// </summary>
		public string ListSelectedItem
		{
			get { return listSelectedItem; }
			set { OnPropertyChanged(ref listSelectedItem, value); }
		}

		public IEnumerable<string> SelectedItems { get { return selectedItems; } }
		public ObservableCollection<ListViewSelectableItemViewModel> Items { get; private set; }

		public ListViewMultiSelectItemsLimitedCountViewModel(int _amountToBeSelected, List<string> _possibleOptions)
		{
			amountToBeSelected = _amountToBeSelected;
			possibleOptions = _possibleOptions;
			selectedItems = new List<string>();
			Items = new ObservableCollection<ListViewSelectableItemViewModel>();
			listSelectedItem = String.Empty;
			PopulateItems();
		}

		public void PopulateItems(int _amountToBeSelected, List<string> _possibleOptions)
		{
			possibleOptions = _possibleOptions;
			AmountToBeSelected = _amountToBeSelected;
			AmountSelected = 0;
			selectedItems.Clear();
			PopulateItems();
		}

		private void PopulateItems()
		{
			Items.Clear();
			foreach (var item in possibleOptions)
			{
				ListViewSelectableItemViewModel selectableItemVM = new ListViewSelectableItemViewModel(item);
				selectableItemVM.OnSelect += AddSelectedItem;
				selectableItemVM.OnDeselect += RemoveSelectedItem;
				Items.Add(selectableItemVM);
			}
		}

		private void AddSelectedItem(ListViewSelectableItemViewModel itemToAdd)
		{
			AmountSelected += 1;
			if (AmountSelected > amountToBeSelected)
			{
				itemToAdd.IsSelected = false;
				return;
			}

			selectedItems.Add(itemToAdd.BoundItem);
		}

		private void RemoveSelectedItem(ListViewSelectableItemViewModel itemToRemove)
		{
			int temp = AmountSelected - 1;
			if (temp < 0)
				return;

			selectedItems.Remove(itemToRemove.BoundItem);
			AmountSelected = temp;
		}
	}
}
