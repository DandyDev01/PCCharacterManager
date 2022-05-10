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

				if (selectedItem != null)
				{
					SelectedItemProperties.Clear();
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
				SelectedItem = filteredItems[0];
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

		public ObservableCollection<ItemDisplayViewModel> ItemsToShow { get; private set; }

		private List<ItemDisplayViewModel> filteredItems;

		public CharacterInventoryViewModel(CharacterStore _characterStore, ICharacterDataService dataService)
			: base(_characterStore, dataService)
		{
			AddItemCommand = new RelayCommand(AddItemWindow);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);

			ItemsToShow = new ObservableCollection<ItemDisplayViewModel>();
			filteredItems = new List<ItemDisplayViewModel>();
			prevSelectedProperty = new PropertyEditableViewModel(new Property());

			searchTerm = string.Empty;

			characterStore.SelectedCharacterChange += OnCharacterChanged;

			SelectedItemProperties = new ObservableCollection<PropertyEditableViewModel>();
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			selectedCharacter = newCharacter;
			Inventory.SortByTag(selectedCharacter.Inventory);

			ItemsToShow.Clear();
			SelectedItemProperties.Clear();
			SelectedProperty = null;

			foreach (var item in selectedCharacter.Inventory.All.OrderBy(x => x.Name))
			{
				if (item.Tag == selectedFilter)
				{
					ItemDisplayViewModel itemVM = new ItemDisplayViewModel(item);
					//itemVM.RemoveAction += selectedCharacter.Inventory.Remove;
					ItemsToShow.Add(itemVM);
				}
			}
			if(ItemsToShow.Count > 0)
				SelectedItem = ItemsToShow[0];
		}

		private void AddItemWindow()
		{
			Window window = new AddItemDialogWindow();
			window.DataContext =
				new DialogWindowAddItemViewModel(dataService, characterStore,
				window, selectedCharacter);

			window.ShowDialog();

			ItemsToShow.Add(new ItemDisplayViewModel(selectedCharacter.Inventory.All.Last()));

		}

		private void Search()
		{
			ItemsToShow.Clear();

			if (searchTerm == string.Empty || string.IsNullOrWhiteSpace(SearchTerm))
			{
				ItemsToShow = new ObservableCollection<ItemDisplayViewModel>(filteredItems);
				OnPropertyChaged("ItemsToShow");
			}
			else
			{
				foreach (var item in filteredItems)
				{
					if (item.BoundItem.Name.ToLower().Contains(searchTerm.ToLower()))
					{
						ItemsToShow.Add(item);
						continue;
					}

					if (item.BoundItem.Properties != null)
					{
						foreach (var property in item.BoundItem.Properties)
						{
							if (property.Name.ToLower().Contains(SearchTerm.ToLower()))
							{
								ItemsToShow.Add(item);
								break;
							}
						}
					}

				} // end for
			} // end if

		} // end search

		private void Filter()
		{
			ItemsToShow.Clear();

			foreach (var item in selectedCharacter.Inventory.All.OrderBy(x => x.Name))
			{

				if (item.Tag.ToString().ToLower().Contains(selectedFilter.ToString().ToLower()))
				{
					ItemDisplayViewModel temp = new ItemDisplayViewModel(item);
					ItemsToShow.Add(temp);
				}

			} // end for

			if (ItemsToShow.Count < 1)
			{
				var messageBox = MessageBox.Show("There are no items of the type " + selectedFilter.ToString(), "", MessageBoxButton.OK);
				SelectedFilter = ItemType.Weapon;
				return;
			}

			filteredItems = ItemsToShow.ToList();

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
