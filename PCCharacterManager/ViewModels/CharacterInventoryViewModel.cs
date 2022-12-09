using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterInventoryViewModel : TabItemViewModel
	{
		public ICommand AddItemCommand { get; private set; }
		public ICommand RemoveItemCommand { get; private set; }
		public ICommand AddPropertyCommand { get; private set; }
		public ICommand RemovePropertyCommand { get; private set; }
		public ICommand NextFilterCommand { get; private set; }

		public ICollectionView ItemsCollectionView { get; private set; }
		public ObservableCollection<ItemDisplayViewModel> ItemDisplayVms { get; private set; }

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


		public ItemType[] Filters { get; private set; } = (ItemType[])Enum.GetValues(typeof(ItemType));
		private ItemType selectedFilter;
		public ItemType SelectedFilter
		{
			get { return selectedFilter; }
			set
			{
				OnPropertyChaged(ref selectedFilter, value);
				ItemsCollectionView.Refresh();
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
				ItemsCollectionView.Refresh();
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

		public CharacterInventoryViewModel(CharacterStore _characterStore, ICharacterDataService dataService)
			: base(_characterStore, dataService)
		{
			AddItemCommand = new AddItemToInventoryCommand(this);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);
			NextFilterCommand = new RelayCommand(NextFilter);

			ItemDisplayVms = new ObservableCollection<ItemDisplayViewModel>();
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = FilterItems;
			ItemsCollectionView.SortDescriptions.Add(new SortDescription(nameof(ItemDisplayViewModel.DisplayName), ListSortDirection.Ascending));

			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

			searchTerm = string.Empty;
			search = new ItemSearch();

			characterStore.SelectedCharacterChange += OnCharacterChanged;

			PropertiesToDisplay = new ObservableCollection<PropertyEditableViewModel>();
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			selectedCharacter = newCharacter;

			ItemDisplayVms.Clear();
			PropertiesToDisplay.Clear();
			SelectedProperty = null;


			foreach (var pair in selectedCharacter.Inventory.Items)
			{
				foreach (var item in pair.Value)
				{
					ItemDisplayVms.Add(new ItemDisplayViewModel(item));
				}
			}

			if (ItemDisplayVms.Count > 0)
				SelectedItem = ItemDisplayVms[0];

			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
		}

		private bool FilterItems(object obj)
		{
			if(obj is ItemDisplayViewModel displayVM)
			{
				Item item = displayVM.BoundItem;
				if (!item.Tag.Equals(selectedFilter)) return false;

				if (searchTerm.Equals(string.Empty)) return true;

				if (item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;

				foreach (Property property in item.Properties)
				{
					if (property.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;
					if (property.Desc.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;
				}
			}

			return false;
		}

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

		private void NextFilter()
		{
			int currentIndex = (int)selectedFilter;
			int nextIndex = currentIndex + 1;
			if(nextIndex > Filters.Length -1)
			{
				currentIndex = 0;
				SelectedFilter = Filters[currentIndex];
				return;
			} 

			SelectedFilter = Filters[nextIndex];
		}
	} // end class
}
