using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddItemViewModel : TabItemViewModel
	{
		private Window addItemWindow; // need this in order to close the window
		public Array ItemTypes { get; private set; } = Enum.GetValues(typeof(ItemType));
		public ObservableCollection<ItemEditableViewModel> ItemsToDisplay { get; set; }

		private ItemType selectedItemType;
		// has the code to update the items in the ItemListing
		// probably change this
		public ItemType SelectedItemType
		{
			get { return selectedItemType; }
			set
			{
				OnPropertyChaged(ref selectedItemType, value);
				// Change the item listing
				ItemsToDisplay.Clear();
				foreach (var item in PopulateItemsToDisplay())
				{
					ItemsToDisplay.Add(item);
				}

			}
		}

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref searchTerm, value);
				Search();
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

		private List<ItemEditableViewModel> allItemsViewModel;
		private ItemSearch itemSearch;

		public DialogWindowAddItemViewModel(ICharacterDataService dataService, CharacterStore characterStore,
			Window addItemWindow, Character character) : base(characterStore, dataService, character)
		{
			ItemsToDisplay = new ObservableCollection<ItemEditableViewModel>();
			this.addItemWindow = addItemWindow;
			selectedCharacter = character;
			itemSearch = new ItemSearch();
			AddToInventoryCommand = new RelayCommand(AddItem);
			CancelCommand = new RelayCommand(Close);

			List<Item> allItems = ReadWriteJsonCollection<Item>.ReadCollection(Resources.AllItemsJson);
			allItemsViewModel = new List<ItemEditableViewModel>();

			foreach (Item item in allItems)
			{
				allItemsViewModel.Add(new ItemEditableViewModel(item));
			}

			selectedItem = new ItemEditableViewModel(allItems[0]);
		}

		private void AddItem()
		{
			addItemWindow.Close();
		}

		private void Close()
		{
			addItemWindow.Close();
		}

		private void Search()
		{
			var results = itemSearch.Search(SearchTerm, PopulateItemsToDisplay());

			ItemsToDisplay.Clear();
			foreach (var item in results)
			{
				ItemsToDisplay.Add((ItemEditableViewModel)item);
			}
		}

		private List<ItemEditableViewModel> PopulateItemsToDisplay()
		{
			return allItemsViewModel.FindAll(item => item.BoundItem.Tag == selectedItemType);
		}
	}
}
