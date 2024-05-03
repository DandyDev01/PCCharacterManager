using PCCharacterManager.DialogWindows;
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

		public StringListViewModel(string listName, ObservableCollection<string> item)
		{
			_listName = listName;
			ItemsToDisplay = item;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
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
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window,
				"enter text");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
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
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel(window, "Edit ");

			if (_selectedItem == null)
				return;

			windowVM.Answer = _selectedItem;
			window.DataContext = windowVM;
			window.ShowDialog();


			if (window.DialogResult == false)
				return;

			ItemsToDisplay.Remove(_selectedItem);

			_selectedItem = windowVM.Answer;
			ItemsToDisplay.Add(_selectedItem);
		}

	}
}
