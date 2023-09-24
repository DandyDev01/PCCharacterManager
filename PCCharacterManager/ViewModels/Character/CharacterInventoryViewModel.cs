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
		private readonly PropertyEditableVMPool propertyVMPool;
		private readonly CollectionViewPropertySort collectionViewPropertySort;
		private readonly ItemSearch itemSearch;
		public Array ItemCategories { get; } = Enum.GetValues(typeof(ItemCategory));
		public Array ItemTypes { get; } = Enum.GetValues(typeof(ItemType));
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
		public ICommand CategorySortCommand { get; }

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
					// is in the display list, remove it.
					if (PropertiesToDisplay.Contains(PrevSelectedProperty))
					{
						PropertiesToDisplay.Remove(PrevSelectedProperty);
					}
				}
			}
		}

		public ObservableCollection<PropertyEditableViewModel> PropertiesToDisplay { get; }
		public PropertyEditableViewModel? PrevSelectedProperty { get; private set; }

		public string SearchTerm
		{
			get => itemSearch.SearchTerm;
			set
			{
				itemSearch.SearchTerm = value;
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

		public CharacterInventoryViewModel()
		{
			propertyVMPool = new PropertyEditableVMPool(5);
			itemSearch = new ItemSearch();

			AddItemCommand = new AddItemToInventoryCommand(this);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);

			ItemDisplayVms = new ObservableCollection<ItemViewModel>();
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = itemSearch.Search;
			collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemCategory), ListSortDirection.Ascending));

			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

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
			CategorySortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemCategory));
		}

		public CharacterInventoryViewModel(CharacterStore _characterStore) : this()
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;
		}

		public CharacterInventoryViewModel(ObservableCollection<ItemViewModel> _itemsToDisplay) : this()
		{
			ItemDisplayVms = _itemsToDisplay;
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = itemSearch.Search;
			collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemCategory), ListSortDirection.Ascending));

			NameSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayName));
			CostSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayCost));
			WeightSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayWeight));
			QuantitySortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayQuantity));
			TypeSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemCategory));
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
					ItemViewModel itemVM = new(item);
					ItemDisplayVms.Add(itemVM);
				}
			}

			if (ItemDisplayVms.Count > 0)
				SelectedItem = ItemDisplayVms[0];

			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
		}

		public void ReturnItemVMsToPool()
		{
			foreach (PropertyEditableViewModel propertyEditableVM in PropertiesToDisplay)
			{
				propertyVMPool.Return(propertyEditableVM);
			}
		}

		private void PopulatePropertiesToDisplay()
		{
			PropertiesToDisplay.Clear();

			if (selectedItem == null || selectedItem.BoundItem == null) 
				return;

			if (selectedItem.BoundItem.Properties == null)
				return;

			foreach (var property in selectedItem.BoundItem.Properties)
			{
				PropertyEditableViewModel editablePropertyVM = propertyVMPool.GetItem();
				
				// only show properties that are not marked HIDDEN
				if (!showHiddenProperties)
				{
					if (property.Hidden)
						continue;

					editablePropertyVM.Bind(property);
					PropertiesToDisplay.Add(editablePropertyVM);
				}
				else
				{
					editablePropertyVM.Bind(property);
					PropertiesToDisplay.Add(editablePropertyVM);
				}
			} // end loop
		} // end method

	} // end class
}
