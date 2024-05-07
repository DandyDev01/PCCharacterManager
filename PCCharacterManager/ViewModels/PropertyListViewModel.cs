using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using PCCharacterManager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PCCharacterManager.Services;

namespace PCCharacterManager.ViewModels
{
	public class PropertyListViewModel : ObservableObject
	{
		private readonly DialogServiceBase _dialogService;

		private string _listName;
		public string ListName
		{
			get
			{
				return _listName;
			}
			private set
			{
				OnPropertyChanged(ref _listName, value);
			}
		}

		private Property? _selectedItem;
		public Property? SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				OnPropertyChanged(ref _selectedItem, value);
			}
		}

		public ObservableCollection<Property> ItemsToDisplay { get; private set; }

		public ICommand AddItemCommand { get; protected set; }
		public ICommand RemoveItemCommand { get; protected set; }
		public ICommand EditItemCommand { get; protected set; }
		public ICommand PropertyItemReceivedCommand { get; }

		public Action<Property>? OnAddItem;
		public Action<Property>? OnRemoveItem;

		public PropertyListViewModel(string listName, ObservableCollection<Property> item, DialogServiceBase dialogService)
		{
			_listName = listName;
			ItemsToDisplay = item;
			_dialogService = dialogService;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();
		}

		public PropertyListViewModel(string listName, DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
			_listName = listName;
			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();

			ItemsToDisplay = new();
		}

		public void UpdateCollection(ObservableCollection<Property> items)
		{
			ItemsToDisplay = items;

			OnPropertyChanged(nameof(ItemsToDisplay));
		}

		/// <summary>
		/// Add item to provided ObservableCollection
		/// </summary>
		private void AddItem()
		{
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel("Name");
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;


			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel("Description");
			result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM1, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			Property property = new Property(windowVM.Answer, windowVM1.Answer);
			ItemsToDisplay.Add(property);
			OnAddItem?.Invoke(property);
		}

		/// <summary>
		/// Remove item from provided ObservableCollection
		/// </summary>
		private void RemoveItem()
		{
			if (_selectedItem is null)
				return;

			ItemsToDisplay.Remove(_selectedItem);
			OnRemoveItem?.Invoke(_selectedItem);
		}

		/// <summary>
		/// Edit an item in provided ObserbvableCollection
		/// </summary>
		private void EditItem()
		{
			if (_selectedItem is null)
				return;

			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel("Edit " + _selectedItem.Name);

			windowVM.Answer = _selectedItem.Desc;
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedItem.Desc = windowVM.Answer;
		}

	}
}
