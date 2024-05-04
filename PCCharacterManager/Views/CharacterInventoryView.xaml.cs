using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterInventoryView.xaml
	/// </summary>
	public partial class CharacterInventoryView : UserControl
	{
		private ICommand focusSearchCommand;
		private ICommand deleteSelectedItemsCommand;
		private ICommand addItemCommand;
		private double lastItemRemoveTimeInSeconds;

		public CharacterInventoryView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			deleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItems);
			removeContextButton.Command = new RelayCommand(DeleteSelectedItems);
			delKeyBinding.Command = new RelayCommand(DeleteSelectedItems);
			addItemCommand = new RelayCommand(AddItem);
			
			removeButton.Command = deleteSelectedItemsCommand;

			InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
			InputBindings.Add(new KeyBinding(addItemCommand, Key.OemPlus, ModifierKeys.Control));
			InputBindings.Add(new KeyBinding(removeButton.Command, Key.OemMinus, ModifierKeys.Control));

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastItemRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
		}

		private void AddItem()
		{
			var vm = DataContext as CharacterInventoryViewModel;

			if (vm == null)
				return;

			vm.AddItemCommand.Execute(this);
		}

		public void FocusSearch()
		{
			placeholderText.Visibility = Visibility.Collapsed;
			searchBox.Visibility = Visibility.Visible;
			this.searchBox.Focus();
		}

		private void DeleteSelectedItems()
		{
			CharacterInventoryViewModel? inventoryVM = DataContext as CharacterInventoryViewModel;

			if (inventoryVM == null || inventoryVM.Inventory == null) 
				return;

			string confirmationBoxMessage = "Are you sure you want to remove " +
					items.SelectedItems.Count + " items";
			string messageBoxCaption = "Remove Items";

			// more than one item is being deleted
			if (items.SelectedItems.Count > 1)
			{
				var confirmationMessageBox = MessageBox.Show(confirmationBoxMessage, 
					messageBoxCaption, MessageBoxButton.YesNo);
				
				if (confirmationMessageBox == MessageBoxResult.No) 
					return;

				List<ItemViewModel> itemVMs = new List<ItemViewModel>();
				
				foreach (var item in items.SelectedItems)
				{
					if (item is not ItemViewModel itemVM ||
						itemVM.BoundItem == null)
						continue;

					inventoryVM.Inventory.Remove(itemVM.BoundItem);
					itemVMs.Add(itemVM);
				}

				foreach (var item in itemVMs)
				{
					inventoryVM.ItemDisplayVms.Remove(item);
				}

				itemVMs.Clear();
			}
			// only one item is being deleted
			else
			{
				if (inventoryVM.SelectedItem == null)
					return;

				double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
				double timePassed = currTimeSeconds - lastItemRemoveTimeInSeconds;
				
				// only ask for confirmation if it has been more than
				// five seconds since last deletion
				if (timePassed > 5)
				{
					confirmationBoxMessage = "Are you sure you want to remove " + inventoryVM.SelectedItem.DisplayName;

					var messageBox = MessageBox.Show(confirmationBoxMessage, messageBoxCaption, MessageBoxButton.YesNo);

					if (messageBox == MessageBoxResult.No)
						return;
				}

				if (inventoryVM.SelectedItem is not ItemViewModel itemVM ||
					itemVM.BoundItem == null)
					return;

				inventoryVM.Inventory.Remove(itemVM.BoundItem);
				inventoryVM.ItemDisplayVms.Remove(inventoryVM.SelectedItem);
			}

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastItemRemoveTimeInSeconds = timeSpan.TotalSeconds;

			if (inventoryVM.ItemDisplayVms.Count < 1) 
				return;
			
			inventoryVM.SelectedItem = inventoryVM.ItemDisplayVms[0];
			inventoryVM.CalculateInventoryWeight();
		}

		private void searchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (searchBox.Text == "")
			{
				placeholderText.Visibility = Visibility.Visible;
				searchBox.Visibility = Visibility.Collapsed;
			}
		}

		private void searchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (searchBox.Text == "")
			{
				placeholderText.Visibility = Visibility.Collapsed;
				searchBox.Visibility = Visibility.Visible;
				FocusSearch();
			}
		}
	}
}
