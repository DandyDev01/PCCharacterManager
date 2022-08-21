using PCCharacterManager.DialogWindows;
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
	public class CharacterInventoryViewModel : TabItemViewModel
	{
		public ICommand AddItemCommand { get; private set; }
		public ICommand RemoveItemCommand { get; private set; }
		public ICommand AddPropertyCommand { get; private set; }
		public ICommand RemovePropertyCommand { get; private set; }

		private ItemDisplayViewModel? selectedItem;
		public ItemDisplayViewModel? SelectedItem
		{
			get { return selectedItem; }
			set
			{
				OnPropertyChaged(ref selectedItem, value);
				SelectedItemProperties.Clear();

				if (selectedItem != null)
				{
					if (selectedItem.BoundItem.Properties != null)
					{
						foreach (var property in selectedItem.BoundItem.Properties)
						{
							SelectedItemProperties.Add(new PropertyEditableViewModel(property));
						}
					}

				}
			}
		}

		private PropertyEditableViewModel? prevSelectedProperty;
		private PropertyEditableViewModel? selectedProperty;
		public PropertyEditableViewModel? SelectedProperty
		{
			get { return selectedProperty; }
			set
			{
				OnPropertyChaged(ref selectedProperty, value);
				selectedProperty?.Edit();
				prevSelectedProperty = selectedProperty;
				selectedProperty = null;
			}
		}

		public ObservableCollection<PropertyEditableViewModel> SelectedItemProperties { get; private set; }

		public Array Filters { get; private set; } = Enum.GetValues(typeof(ItemType));
		private ItemType selectedFilter;
		public ItemType SelectedFilter
		{
			get { return selectedFilter; }
			set
			{
				OnPropertyChaged(ref selectedFilter, value);
				Filter();
			}
		}

		private readonly ItemSearch search;
		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref searchTerm, value);
				searchTerm = searchTerm.ToLower();
				Search();
				//float timePassed = (float)DateTime.Now.Subtract(lastSearchTime).TotalSeconds;
				//searchTimer.Tick(timePassed);
				//lastSearchTime = DateTime.Now;
			}
		}

		public ObservableCollection<ItemDisplayViewModel> ItemsToShow { get; private set; }

		private Dictionary<ItemType, ObservableCollection<ItemDisplayViewModel>> filteredItems;

		public CharacterInventoryViewModel(CharacterStore _characterStore, ICharacterDataService dataService)
			: base(_characterStore, dataService)
		{
			AddItemCommand = new RelayCommand(AddItemWindow);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);

			filteredItems = new Dictionary<ItemType, ObservableCollection<ItemDisplayViewModel>>();
			filteredItems.Add(ItemType.Weapon, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Armor, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Ammunition, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Item, new ObservableCollection<ItemDisplayViewModel>());
			ItemsToShow = new ObservableCollection<ItemDisplayViewModel>();
			prevSelectedProperty = new PropertyEditableViewModel(new Property());

			searchTerm = string.Empty;
			search = new ItemSearch();

			characterStore.SelectedCharacterChange += OnCharacterChanged;

			SelectedItemProperties = new ObservableCollection<PropertyEditableViewModel>();
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			selectedCharacter = newCharacter;
			SetFilteredItemsDictionary();

			ItemsToShow.Clear();
			SelectedItemProperties.Clear();
			SelectedProperty = null;

			Filter();

			if (ItemsToShow.Count > 0)
				SelectedItem = ItemsToShow[0];
		}

		private void Search()
		{
			ItemsToShow.Clear();
			ItemDisplayViewModel[] items = search.Search(searchTerm, filteredItems[selectedFilter]).ToArray();
			foreach (var item in items)
			{
				ItemsToShow.Add(item);
			}

			if (ItemsToShow.Count > 0)
				SelectedItem = ItemsToShow[0];
		} // end search

		private void SetFilteredItemsDictionary()
		{
			foreach (var item in filteredItems)
			{
				item.Value.Clear();
			}

			foreach (var pair in selectedCharacter.Inventory.Items)
			{
				foreach (var item in pair.Value)
				{
					filteredItems[item.Tag].Add(new ItemDisplayViewModel(item));
				}
			}

			filteredItems[ItemType.Item] = new ObservableCollection<ItemDisplayViewModel>(filteredItems[ItemType.Item].OrderBy(x => x.ItemName));
			filteredItems[ItemType.Weapon] = new ObservableCollection<ItemDisplayViewModel>(filteredItems[ItemType.Weapon].OrderBy(x => x.ItemName));
			filteredItems[ItemType.Armor] = new ObservableCollection<ItemDisplayViewModel>(filteredItems[ItemType.Armor].OrderBy(x => x.ItemName));
			filteredItems[ItemType.Ammunition] = new ObservableCollection<ItemDisplayViewModel>(filteredItems[ItemType.Ammunition].OrderBy(x => x.ItemName));
		}

		private void Filter()
		{
			ItemsToShow.Clear();
			foreach (var item in filteredItems[selectedFilter])
			{
				ItemsToShow.Add(item);
			}

			if(ItemsToShow.Count > 0)
				SelectedItem = ItemsToShow[0];
		} // end Filter

		private void AddProperty()
		{
			if (SelectedItem == null)
				return;

			SelectedItem.BoundItem.AddProperty(new Property("name", "desc"));
			SelectedItemProperties.Clear();
			foreach (var property in SelectedItem.BoundItem.Properties)
			{
				SelectedItemProperties.Add(new PropertyEditableViewModel(property));
			}
		}

		private void RemoveProperty()
		{
			if (SelectedItem == null || prevSelectedProperty == null)
				return;

			SelectedItem.BoundItem.RemoveProperty(prevSelectedProperty.BoundProperty);
			SelectedItemProperties.Remove(prevSelectedProperty);
		}

		private void AddItemWindow()
		{
			Window window = new AddItemDialogWindow();
			DialogWindowAddItemViewModel dialogContext =
				new DialogWindowAddItemViewModel(dataService, characterStore, window, selectedCharacter);
			window.DataContext = dialogContext;

			window.ShowDialog();

			ItemsToShow.Add(new ItemDisplayViewModel(dialogContext.SelectedItem.BoundItem));
			ItemsToShow.OrderBy(x => x.ItemName);
		}

		private void RemoveItem()
		{
			if (SelectedItem == null)
				return;

			var messageBox = MessageBox.Show("Are you sure you want to remove " + SelectedItem.ItemName, "Remove Item", MessageBoxButton.YesNo);

			if (messageBox == MessageBoxResult.No)
				return;

			selectedCharacter.Inventory.Remove(SelectedItem.BoundItem);
			ItemsToShow.Remove(SelectedItem);
		}
	} // end class
}
