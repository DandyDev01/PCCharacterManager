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
		private double lastItemRemoveTimeInSeconds;

		public CharacterInventoryView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			deleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItems);
			removeContextButton.Command = new RelayCommand(DeleteSelectedItems);
			delKeyBinding.Command = new RelayCommand(DeleteSelectedItems);

			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
			removeButton.Command = deleteSelectedItemsCommand;
			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastItemRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}

		private void DeleteSelectedItems()
		{
			CharacterInventoryViewModel vm = DataContext as CharacterInventoryViewModel;

			if (vm == null) return;

			if(items.SelectedItems.Count > 1)
			{
				var messageBox1 = MessageBox.Show("Are you sure you want to remove " + 
					items.SelectedItems.Count + " items", "Remove Items", MessageBoxButton.YesNo);
				if (messageBox1 == MessageBoxResult.No) return;

				List<ItemViewModel> tempItems = new List<ItemViewModel>();
				foreach (var item in items.SelectedItems)
				{
					ItemViewModel temp = item as ItemViewModel;
					if (temp == null) continue;
					vm.Inventory.Remove(temp.BoundItem);
					tempItems.Add(temp);
				}

				foreach (var item in tempItems)
				{
					vm.ItemDisplayVms.Remove(item);
				}

				tempItems.Clear();
			}
			else
			{
				if (vm.SelectedItem == null)
					return;

				double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
				double timePassed = currTimeSeconds - lastItemRemoveTimeInSeconds;
				if (timePassed > 5)
				{
					var messageBox = MessageBox.Show("Are you sure you want to remove " + 
						vm.SelectedItem.DisplayName, "Remove Item", MessageBoxButton.YesNo);

					if (messageBox == MessageBoxResult.No)
						return;
				}

				vm.Inventory.Remove(vm.SelectedItem.BoundItem);
				vm.ItemDisplayVms.Remove(vm.SelectedItem);
			}

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastItemRemoveTimeInSeconds = timeSpan.TotalSeconds;

			if (vm.ItemDisplayVms.Count < 1) return;
			vm.SelectedItem = vm.ItemDisplayVms[0];
		}
	}
}
