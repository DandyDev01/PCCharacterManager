using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Condition = PCCharacterManager.Models.Condition;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using PCCharacterManager.Services;

namespace PCCharacterManager.ViewModels
{
    public class ConditionListViewModel : ObservableObject
    {
		private DialogService _dialogService;

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

		private Condition? _selectedItem;
		public Condition? SelectedItem
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

		public ObservableCollection<Condition> ItemsToDisplay { get; private set; }

		public ICommand AddItemCommand { get; protected set; }
		public ICommand RemoveItemCommand { get; protected set; }
		public ICommand EditItemCommand { get; protected set; }
		public ICommand PropertyItemReceivedCommand { get; }

		public Action<Condition>? OnAddItem;
		public Action<Condition>? OnRemoveItem;

		public ConditionListViewModel(string listName, ObservableCollection<Condition> item, DialogService dialogService)
		{
			_listName = listName;
			ItemsToDisplay = item;
			_dialogService = dialogService;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();
			_dialogService = dialogService;
		}

		public ConditionListViewModel(string listName, DialogService dialogService)
		{
			_dialogService = dialogService;
			_listName = listName;
			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();

			ItemsToDisplay = new();
		}

		public void UpdateCollection(ObservableCollection<Condition> items)
		{
			ItemsToDisplay = items;

			OnPropertyChanged(nameof(ItemsToDisplay));
		}

		/// <summary>
		/// Add item to provided ObservableCollection
		/// </summary>
		private void AddItem()
		{
			Window window = new AddConditionDialogWindow();
			DialogWindowAddConditionViewModel windowVM = new DialogWindowAddConditionViewModel(window);
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Condition property = new Condition(windowVM.Name, windowVM.Description, windowVM.Duration);
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

			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel("Edit " + _selectedItem.Name);

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
