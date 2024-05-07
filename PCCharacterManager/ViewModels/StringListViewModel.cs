using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
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
	public class StringListViewModel : ObservableObject
	{
		private readonly DialogServiceBase _dialogService;
		private readonly string _listName;
		public string ListName => _listName;

		private string? _selectedItem;
		public string? SelectedItem
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

		public ObservableCollection<string> ItemsToDisplay { get; private set; }

		public ICommand AddItemCommand { get; }
		public ICommand RemoveItemCommand { get; }
		public ICommand EditItemCommand { get; }

		public Action<string>? OnAddItem;
		public Action<string>? OnRemoveItem;

		public StringListViewModel(string listName, ObservableCollection<string> item, DialogServiceBase dialogService)
		{
			_listName = listName;
			ItemsToDisplay = item;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			_dialogService = dialogService;	
		}

		public void UpdateCollection(ObservableCollection<string> items)
		{
			ItemsToDisplay = items;

			OnPropertyChanged(nameof(ItemsToDisplay));
		}

		/// <summary>
		/// Add item to provided ObservableCollection
		/// </summary>
		private void AddItem()
		{
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel("Enter text.");

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			OnAddItem?.Invoke(windowVM.Answer);
			ItemsToDisplay.Add(windowVM.Answer);
		}

		/// <summary>
		/// Remove item from provided ObservableCollection
		/// </summary>ten
		private void RemoveItem()
		{
			if (_selectedItem == null)
				return;

			OnRemoveItem?.Invoke(_selectedItem);
			ItemsToDisplay.Remove(_selectedItem);
		}

		/// <summary>
		/// Edit an item in provided ObserbvableCollection
		/// </summary>
		private void EditItem()
		{
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel("Edit ");

			windowVM.Answer = _selectedItem;
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});


			if (result == false.ToString())
				return;

			ItemsToDisplay.Remove(_selectedItem);

			_selectedItem = windowVM.Answer;
			ItemsToDisplay.Add(_selectedItem);
		}

	}
}
