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
		private readonly PropertyEditableVMPool _propertyVMPool;
		private readonly CollectionViewPropertySort _collectionViewPropertySort;
		private readonly ItemSearch _itemSearch;

		public Array ItemCategories { get; } = Enum.GetValues(typeof(ItemCategory));
		public Array ItemTypes { get; } = Enum.GetValues(typeof(ItemType));
		public Inventory Inventory { get; private set; }
		public string SearchTerm
		{
			get => _itemSearch.SearchTerm;
			set
			{
				_itemSearch.SearchTerm = value;
				ItemsCollectionView.Refresh();
			}
		}

		public ObservableCollection<ItemViewModel> ItemDisplayVms { get; }
		public ICollectionView ItemsCollectionView { get; private set; }

		private ItemViewModel? _selectedItem;
		public ItemViewModel? SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				OnPropertyChanged(ref _selectedItem, value);
				PopulatePropertiesToDisplay();
			}
		}
		
		private PropertyEditableViewModel? _selectedProperty;
		public PropertyEditableViewModel? SelectedProperty
		{
			get { return _selectedProperty; }
			set
			{
				OnPropertyChanged(ref _selectedProperty, value);
				_selectedProperty?.Edit();

				if (_selectedProperty == null) return;

				PrevSelectedProperty = _selectedProperty;
				_selectedProperty = null;

				if (_showHiddenProperties) return;
				
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

		private string _showHiddenPropertiesText;
		public string ShowHiddenPropertiesText
		{
			get
			{
				return _showHiddenPropertiesText;
			}
			set
			{
				OnPropertyChanged(ref _showHiddenPropertiesText, value);
			}
		}

		private string _inventoryWeight;
		public string InventoryWeight => _inventoryWeight;

		private bool _showHiddenProperties = false;
		private CharacterBase _selectedCharacter;

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

		public ICommand ShowPropertiesToDisplayCommand { get; }

		public CharacterInventoryViewModel(CharacterStore characterStore, DialogServiceBase dialogService, RecoveryBase recovery)
		{
			_recovery = recovery;
			_selectedCharacter = characterStore.SelectedCharacter;
			Inventory = characterStore.SelectedCharacter.Inventory;
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_propertyVMPool = new PropertyEditableVMPool(5);
			_itemSearch = new ItemSearch();

			_inventoryWeight = string.Empty;
			_showHiddenPropertiesText = string.Empty;

			AddItemCommand = new AddItemToInventoryCommand(this, dialogService);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);

			ItemDisplayVms = new ObservableCollection<ItemViewModel>();
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = _itemSearch.Search;
			_collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemCategory), ListSortDirection.Ascending));

			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

			PropertiesToDisplay = new ObservableCollection<PropertyEditableViewModel>();

			ShowPropertiesToDisplayCommand = new RelayCommand(PopulatePropertiesToDisplay);

			NameSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayName));
			CostSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayCost));
			WeightSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayWeight));
			QuantitySortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayQuantity));
			TypeSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemType));
			CategorySortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemCategory));
		}

		public CharacterInventoryViewModel(ObservableCollection<ItemViewModel> itemsToDisplay, DialogServiceBase dialogService)
		{
			_propertyVMPool = new PropertyEditableVMPool(5);
			_itemSearch = new ItemSearch();
			_selectedCharacter = DnD5eCharacter.Default;

			Inventory = new();
			_inventoryWeight = string.Empty;
			_showHiddenPropertiesText = string.Empty;

			AddItemCommand = new AddItemToInventoryCommand(this, dialogService);
			RemoveItemCommand = new RemoveItemFromInventoryCommand(this);
			AddPropertyCommand = new AddPropertyToItemCommand(this);
			RemovePropertyCommand = new RemovePropertyFromItemCommand(this);

			ItemDisplayVms = itemsToDisplay;
			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			ItemsCollectionView.Filter = _itemSearch.Search;
			_collectionViewPropertySort = new CollectionViewPropertySort(ItemsCollectionView);
			ItemsCollectionView.SortDescriptions.Add(
				new SortDescription(nameof(ItemViewModel.DisplayItemCategory), ListSortDirection.Ascending));

			PrevSelectedProperty = new PropertyEditableViewModel(new Property());

			PropertiesToDisplay = new ObservableCollection<PropertyEditableViewModel>();

			ShowPropertiesToDisplayCommand = new RelayCommand(PopulatePropertiesToDisplay);

			NameSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayName));
			CostSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayCost));
			WeightSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayWeight));
			QuantitySortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayQuantity));
			TypeSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemType));
			CategorySortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(ItemViewModel.DisplayItemCategory));
		}

		private void OnCharacterChanged(CharacterBase newCharacter)
		{
			Inventory = newCharacter.Inventory;
			_selectedCharacter = newCharacter;

			ReturnItemVMsToPool();

			foreach (var item in ItemDisplayVms)
			{
				item.PropertyChanged -= HandleInventoryWeightChange;
			}

			ItemDisplayVms.Clear();
			PropertiesToDisplay.Clear();
			SelectedProperty = null;


			foreach (var pair in Inventory.Items)
			{
				foreach (var item in pair.Value)
				{
					ItemViewModel itemVM = new(item);
					itemVM.PropertyChanged += HandleInventoryWeightChange;
					ItemDisplayVms.Add(itemVM);
				}
			}

			if (ItemDisplayVms.Count > 0)
				SelectedItem = ItemDisplayVms[0];

			ItemsCollectionView = CollectionViewSource.GetDefaultView(ItemDisplayVms);
			CalculateInventoryWeight();
		}

		private void HandleInventoryWeightChange(object? sender, PropertyChangedEventArgs e)
		{
			CalculateInventoryWeight();
		}

		private void PopulatePropertiesToDisplay()
		{
			_showHiddenProperties = !_showHiddenProperties;

			if (_showHiddenProperties)
			{
				ShowHiddenPropertiesText = "Showing Hidden Properties";
			}
			else
			{
				ShowHiddenPropertiesText = "Don't show hidden properties";
			}

			PropertiesToDisplay.Clear();

			if (_selectedItem == null || _selectedItem.BoundItem == null) 
				return;

			if (_selectedItem.BoundItem.Properties == null)
				return;

			foreach (var property in _selectedItem.BoundItem.Properties)
			{
				PropertyEditableViewModel editablePropertyVM = _propertyVMPool.GetItem();

				// only show properties that are not marked HIDDEN
				if (_showHiddenProperties == false && property.Hidden)
					continue;

				editablePropertyVM.Bind(property);
				PropertiesToDisplay.Add(editablePropertyVM);
			} // end loop
		} // end method

		public void ReturnItemVMsToPool()
		{
			foreach (PropertyEditableViewModel propertyEditableVM in PropertiesToDisplay)
			{
				_propertyVMPool.Return(propertyEditableVM);
			}
		}

		public void CalculateInventoryWeight()
		{
			int inventoryWeight = 0, length;
			StringBuilder number = new();

			foreach (var keyValuePair in Inventory.Items)
			{
				foreach (var item in keyValuePair.Value)
				{
					length = item.Weight.IndexOf(" ");
					
					if (length < 0)
						length = item.Weight.Length;
					
					number.Clear();
					number.Append(item.Weight.Substring(0, length));
					if (int.TryParse(number.ToString(), out length))
						inventoryWeight += length;
				}
			}

			_inventoryWeight = inventoryWeight + "/" + _selectedCharacter.CarryWeight;
			OnPropertyChanged(nameof(InventoryWeight));
		}
	} // end class
}
