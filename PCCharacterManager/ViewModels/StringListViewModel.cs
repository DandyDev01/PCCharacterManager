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
		private string listName;
		public string ListName
		{
			get
			{
				return listName;
			}
			private set
			{
				OnPropertyChanged(ref listName, value);
			}
		}

		private string? selectedItem;
		public string? SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				OnPropertyChanged(ref selectedItem, value);
			}
		}

		public ObservableCollection<string> ItemsToDisplay { get; }

		public ICommand AddItemCommand { get; protected set; }
		public ICommand RemoveItemCommand { get; protected set; }
		public ICommand EditItemCommand { get; protected set; }

		public StringListViewModel(string _listName, ObservableCollection<string> _item)
		{
			listName = _listName;
			ItemsToDisplay = _item;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
		}

		public void UpdateCollection(ObservableCollection<string> _items)
		{
			ItemsToDisplay.Clear();

			foreach (var item in _items)
			{
				ItemsToDisplay.Add(item);
			}
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

			ItemsToDisplay.Add(windowVM.Answer);
		}

		/// <summary>
		/// Remove item from provided ObservableCollection
		/// </summary>
		private void RemoveItem()
		{
			ItemsToDisplay.Remove(selectedItem);
		}

		/// <summary>
		/// Edit an item in provided ObserbvableCollection
		/// </summary>
		private void EditItem()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel(window, "Edit ");

			windowVM.Answer = selectedItem;
			window.DataContext = windowVM;
			window.ShowDialog();


			if (window.DialogResult == false)
				return;

			ItemsToDisplay.Remove(selectedItem);

			selectedItem = windowVM.Answer;
			ItemsToDisplay.Add(selectedItem);
		}

	}
}
