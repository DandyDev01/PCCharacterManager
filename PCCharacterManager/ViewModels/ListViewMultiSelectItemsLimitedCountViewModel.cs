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
		private List<string> _possibleOptions;
		private List<string> _selectedItems;
		private int _amountToBeSelected;

		public int AmountSelected { get; private set; }
		public int AmountToBeSelected
		{
			get { return _amountToBeSelected; }
			set { OnPropertyChanged(ref _amountToBeSelected, value); }
		}


		public IEnumerable<string> SelectedItems { get { return _selectedItems; } }
		public ObservableCollection<ListViewSelectableItemViewModel> Items { get; private set; }

		public ListViewMultiSelectItemsLimitedCountViewModel(int amountToBeSelected, List<string> possibleOptions)
		{
			_amountToBeSelected = amountToBeSelected;
			_possibleOptions = possibleOptions;
			_selectedItems = new List<string>();
			Items = new ObservableCollection<ListViewSelectableItemViewModel>();

			PopulateItems();
		}

		public void PopulateItems(int amountToBeSelected, List<string> possibleOptions)
		{
			_possibleOptions = possibleOptions;
			AmountToBeSelected = amountToBeSelected;
			AmountSelected = 0;
			_selectedItems.Clear();
			PopulateItems();
		}

		private void PopulateItems()
		{
			Items.Clear();
			foreach (var item in _possibleOptions)
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
			if (AmountSelected > _amountToBeSelected)
			{
				itemToAdd.Toggle();
				return;
			}

			_selectedItems.Add(itemToAdd.BoundItem);
		}

		private void RemoveSelectedItem(ListViewSelectableItemViewModel itemToRemove)
		{
			int temp = AmountSelected - 1;
			if (temp < 0)
				return;

			_selectedItems.Remove(itemToRemove.BoundItem);
			AmountSelected = temp;
		}
	}
}
