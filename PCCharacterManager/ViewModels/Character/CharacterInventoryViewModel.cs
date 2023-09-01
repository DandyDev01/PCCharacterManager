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
	/// <summary>
	/// logic for displaying characters inventory contents
	/// </summary>
	public class CharacterInventoryViewModel : ObservableObject
	{
		private readonly ItemVMPool itemVMPool;
		private readonly PropertyEditableVMPool propertyVMPool;
		private readonly CollectionViewPropertySort collectionViewPropertySort;
		public Inventory? Inventory { get; private set; }

		public ICommand AddItemCommand { get; }
		public ICommand RemoveItemCommand { get; }
		public ICommand AddPropertyCommand { get; }
		public ICommand RemovePropertyCommand { get; }

		public ICommand NameSortCommand { get; }
		public ICommand CostSortCommand { get; }
		public ICommand QuantitySortCommand { get; }
		public ICommand TypeSortCommand { get; }
		public ICommand WeightSortCommand { get; }

		public ICollectionView ItemsCollectionView { get; private set; }
		public ObservableCollection<ItemViewModel> ItemDisplayVms { get; }

		private ItemViewModel? selectedItem;
		public ItemViewModel? SelectedItem
		{
			get { return selectedItem; }
			set
			{
				OnPropertyChanged(ref selectedItem, value);
				PopulatePropertiesToDisplay();
			}
		}
		
		private PropertyEditableViewModel? selectedProperty;
		public PropertyEditableViewModel? SelectedProperty
		{
			get { return selectedProperty; }
			set
			{
				OnPropertyChanged(ref selectedProperty, value);
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

		public ObservableCollection<PropertyEditableViewModel> PropertiesToDisplay { get; }
		public PropertyEditableViewModel? PrevSelectedProperty { get; private set; }

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChanged(ref searchTerm, value);
				ItemsCollectionView.Refresh();
			}
		}

		private bool showHiddenProperties = false;
		public bool ShowHiddenProperties
		{
			get { return showHiddenProperties; }
			set 
			{
				OnPropertyChanged(ref showHiddenProperties, value);
				PopulatePropertiesToDisplay();
			}
		}

		public CharacterInventoryViewModel(CharacterStore _characterStore) : this()
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;
		}

		public CharacterInventoryViewModel(ObservableCollection<ItemViewModel> _itemsToDisplay) : this()
		{
			ItemDisplayVms = _itemsToDisplay;
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = FilterItems;
			collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemType), ListSortDirection.Ascending));

			NameSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayName));
			CostSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayCost));
			WeightSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayWeight));
			QuantitySortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayQuantity));
			TypeSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemType));
		}

		public CharacterInventoryViewModel()
		{
			itemVMPool = new ItemVMPool(10);
			propertyVMPool = new PropertyEditableVMPool(5);

			AddItemCommand = new AddItemToInventoryCommand(this);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);

			ItemDisplayVms = new ObservableCollection<ItemViewModel>();
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = FilterItems;
			collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemType), ListSortDirection.Ascending));

			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

			searchTerm = string.Empty;

			PropertiesToDisplay = new ObservableCollection<PropertyEditableViewModel>();

			NameSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayName));
			CostSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayCost));
			WeightSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayWeight));
			QuantitySortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayQuantity));
			TypeSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemType));
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			Inventory = newCharacter.Inventory;

			ReturnItemVMsToPool();
			ItemDisplayVms.Clear();
			PropertiesToDisplay.Clear();
			SelectedProperty = null;


			foreach (var pair in Inventory.Items)
			{
				foreach (var item in pair.Value)
				{
					ItemViewModel temp = itemVMPool.GetItem();
					temp.Bind(item);
					ItemDisplayVms.Add(temp);
				}
			}

			if (ItemDisplayVms.Count > 0)
				SelectedItem = ItemDisplayVms[0];

			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
		}

		public void ReturnItemVMsToPool()
		{
			foreach (ItemViewModel itemDisplayViewModel in ItemDisplayVms)
			{
				itemVMPool.Return(itemDisplayViewModel);
			}

			foreach (PropertyEditableViewModel propertyEditableVM in PropertiesToDisplay)
			{
				propertyVMPool.Return(propertyEditableVM);
			}
		}

		private bool FilterItems(object obj)
		{
			if(obj is ItemViewModel itemVM)
			{
				if (searchTerm.Equals(string.Empty)) return true;

				if (itemVM.BoundItem.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) return true;
				
				if (itemVM.DisplayItemType.ToString().Contains(SearchTerm, StringComparison.Ordinal)) return true;

				foreach (Property property in itemVM.BoundItem.Properties)
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
				// only show properties that are not marked HIDDEN
				if (!showHiddenProperties)
				{
					if (property.Hidden) continue;

					PropertyEditableViewModel temp1 = propertyVMPool.GetItem();
					temp1.Bind(property);
					PropertiesToDisplay.Add(temp1);

					continue;
				}

				PropertyEditableViewModel temp = propertyVMPool.GetItem();
				temp.Bind(property);
				PropertiesToDisplay.Add(temp);
			}
		}

	} // end class
}
