using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddItemViewModel : TabItemViewModel
	{
		private readonly PropertyEditableVMPool propertyVMPool;
		private readonly ItemEditableVMPool itemVMPool;
		private Window addItemWindow; // need this in order to close the window
		public Array ItemTypes { get; private set; } = Enum.GetValues(typeof(ItemType));
		public ICollectionView ItemsCollectionView { get; private set; }

		private ItemType selectedItemType;
		// has the code to update the items in the ItemListing
		// probably change this
		public ItemType SelectedItemType
		{
			get { return selectedItemType; }
			set
			{
				OnPropertyChaged(ref selectedItemType, value);
				ItemsCollectionView.Refresh();
			}
		}

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref searchTerm, value);
				ItemsCollectionView.Refresh();
			}
		}

		// NOTE: will need this bit right here for making the rest of the editing
		private ItemEditableViewModel selectedItem;
		public ItemEditableViewModel SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem?.Edit();
				OnPropertyChaged(ref selectedItem, value);
				selectedItem?.Edit();
			}
		}

		public ICommand AddToInventoryCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		private ObservableCollection<ItemEditableViewModel> AllItemVMs;

		public DialogWindowAddItemViewModel(ICharacterDataService dataService, CharacterStore characterStore,
			Window _addItemWindow, Character character) : base(characterStore, dataService, character)
		{
			propertyVMPool = new PropertyEditableVMPool(160);
			itemVMPool = new ItemEditableVMPool(74, propertyVMPool);
			addItemWindow = _addItemWindow;
			selectedCharacter = character;
			searchTerm = string.Empty;
			AddToInventoryCommand = new RelayCommand(AddItem);
			CancelCommand = new RelayCommand(Close);

			IEnumerable<Item> allItems = ReadWriteJsonCollection<Item>.ReadCollection(Resources.AllItemsJson);
			AllItemVMs = new ObservableCollection<ItemEditableViewModel>();

			foreach (Item item in allItems)
			{
				ItemEditableViewModel temp = itemVMPool.GetItem();
				temp.Bind(item);
				AllItemVMs.Add(temp);
			}

			ItemsCollectionView = CollectionViewSource.GetDefaultView(AllItemVMs);
			ItemsCollectionView.Filter = FilterItems;
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemEditableViewModel.DisplayName), ListSortDirection.Ascending));

			selectedItem = AllItemVMs[0];
		}

		private void AddItem()
		{
			addItemWindow.DialogResult = true;
			addItemWindow.Close();
		}

		private void Close()
		{
			ReturnToPool();
			addItemWindow.DialogResult = false;
			addItemWindow.Close();
		}

		private void ReturnToPool()
		{
			foreach (var item in AllItemVMs)
			{
				foreach (var propertyEditableViewModel in item.DisplayProperties)
				{
					propertyVMPool.Return(propertyEditableViewModel);
				}

				itemVMPool.Return(item);
			}
		}

		private bool FilterItems(object obj)
		{
			if (obj is ItemViewModel itemVM)
			{
				if (!itemVM.BoundItem.Tag.Equals(selectedItemType)) return false;

				if (searchTerm.Equals(string.Empty)) return true;

				if (itemVM.BoundItem.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;

				foreach (Property property in itemVM.BoundItem.Properties)
				{
					if (property.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;
					if (property.Desc.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;
				}
			}

			return false;
		}
	}
}
