using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
				PopulatePropertiesToDisplay();
			}
		}

		public ObservableCollection<PropertyEditableViewModel> PropertiesToDisplay { get; private set; }
		public PropertyEditableViewModel? PrevSelectedProperty { get; private set; }
		private PropertyEditableViewModel? selectedProperty;
		public PropertyEditableViewModel? SelectedProperty
		{
			get { return selectedProperty; }
			set
			{
				OnPropertyChaged(ref selectedProperty, value);
				selectedProperty?.Edit();

				if (selectedProperty == null) return;

				PrevSelectedProperty = selectedProperty;
				selectedProperty = null;

				if (showHiddenProperties) return;
				
				// property was just marked to be hidden
				if (PrevSelectedProperty.BoundProperty.Hidden)
				{
					// is in the display listm, remove it.
					if (PropertiesToDisplay.Contains(PrevSelectedProperty))
					{
						PropertiesToDisplay.Remove(PrevSelectedProperty);
					}
				}
			}
		}

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

		private bool showHiddenProperties = false;
		public bool ShowHiddenProperties
		{
			get { return showHiddenProperties; }
			set 
			{
				OnPropertyChaged(ref showHiddenProperties, value);
				PopulatePropertiesToDisplay();
			}
		}

		public ObservableCollection<ItemDisplayViewModel> ItemsToShow { get; private set; }
		private Dictionary<ItemType, ObservableCollection<ItemDisplayViewModel>> filteredItems;

		public CharacterInventoryViewModel(CharacterStore _characterStore, ICharacterDataService dataService)
			: base(_characterStore, dataService)
		{
			AddItemCommand = new AddItemToInventoryCommand(this);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);

			filteredItems = new Dictionary<ItemType, ObservableCollection<ItemDisplayViewModel>>();
			filteredItems.Add(ItemType.Weapon, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Armor, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Ammunition, new ObservableCollection<ItemDisplayViewModel>());
			filteredItems.Add(ItemType.Item, new ObservableCollection<ItemDisplayViewModel>());
			ItemsToShow = new ObservableCollection<ItemDisplayViewModel>();
			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

			searchTerm = string.Empty;
			search = new ItemSearch();

			characterStore.SelectedCharacterChange += OnCharacterChanged;

			PropertiesToDisplay = new ObservableCollection<PropertyEditableViewModel>();
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			selectedCharacter = newCharacter;
			SetFilteredItemsDictionary();

			ItemsToShow.Clear();
			PropertiesToDisplay.Clear();
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

		private void PopulatePropertiesToDisplay()
		{
			PropertiesToDisplay.Clear();

			if (selectedItem == null) return;
			if (selectedItem.BoundItem.Properties == null) return;

			foreach (var property in selectedItem.BoundItem.Properties)
			{
				// do not show hidden propertiess
				if (!showHiddenProperties)
				{
					// property is not hidden show it
					if (!property.Hidden)
					{
						PropertiesToDisplay.Add(new PropertyEditableViewModel(property));
					}
				}
				else
				{
					PropertiesToDisplay.Add(new PropertyEditableViewModel(property));
				}

			}
		}
	} // end class
}
