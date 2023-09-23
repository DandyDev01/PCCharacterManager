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

namespace PCCharacterManager.ViewModels
{
	public class PropertyListViewModel : ObservableObject
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

		private Property? selectedItem;
		public Property? SelectedItem
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

		public ObservableCollection<Property> ItemsToDisplay { get; }

		public ICommand AddItemCommand { get; protected set; }
		public ICommand RemoveItemCommand { get; protected set; }
		public ICommand EditItemCommand { get; protected set; }
		public ICommand PropertyItemReceivedCommand { get; }

		public PropertyListViewModel(string _listName, ObservableCollection<Property> _item)
		{
			listName = _listName;
			ItemsToDisplay = _item;

			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();
		}

		public PropertyListViewModel(string listName)
		{
			this.listName = listName;
			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			PropertyItemReceivedCommand = new PropertyItemReceivedCommand();

			ItemsToDisplay = new();
		}

		public void UpdateCollection(ObservableCollection<Property> _items)
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
				"Name");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel(window1, 
				"Description");
			window1.DataContext = windowVM1;
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			Property property = new Property(windowVM.Answer, windowVM1.Answer);
			ItemsToDisplay.Add(property);
		}

		/// <summary>
		/// Remove item from provided ObservableCollection
		/// </summary>
		private void RemoveItem()
		{
			if (selectedItem is null)
				return;

			ItemsToDisplay.Remove(selectedItem);
		}

		/// <summary>
		/// Edit an item in provided ObserbvableCollection
		/// </summary>
		private void EditItem()
		{
			if (selectedItem is null)
				return;

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel(window, "Edit " + selectedItem.Name);

			windowVM.Answer = selectedItem.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedItem.Desc = windowVM.Answer;
		}

	}
}
