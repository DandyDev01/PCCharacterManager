using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class RemoveItemFromInventoryCommand : BaseCommand
	{
		private readonly CharacterInventoryViewModel vm;

		public RemoveItemFromInventoryCommand(CharacterInventoryViewModel _vm)
		{
			vm = _vm;
		}

		public override void Execute(object parameter)
		{
			//if (vm.SelectedItem == null)
			//	return;

			//var messageBox = MessageBox.Show("Are you sure you want to remove " + vm.SelectedItem.DisplayName, "Remove Item", MessageBoxButton.YesNo);

			//if (messageBox == MessageBoxResult.No)
			//	return;

			//vm.Inventory.Remove(vm.SelectedItem.BoundItem);
			//vm.ItemDisplayVms.Remove(vm.SelectedItem);
			//vm.SelectedItem = vm.ItemDisplayVms[0];

			//if (vm == null) return;

			//if (items.SelectedItems.Count > 1)
			//{
			//	var messageBox1 = MessageBox.Show("Are you sure you want to remove " +
			//		items.SelectedItems.Count + " items", "Remove Items", MessageBoxButton.YesNo);
			//	if (messageBox1 == MessageBoxResult.No) return;

			//	List<ItemDisplayViewModel> tempItems = new List<ItemDisplayViewModel>();
			//	foreach (var item in items.SelectedItems)
			//	{
			//		ItemDisplayViewModel temp = item as ItemDisplayViewModel;
			//		if (temp == null) continue;
			//		vm.Inventory.Remove(temp.BoundItem);
			//		tempItems.Add(temp);
			//	}

			//	foreach (var item in tempItems)
			//	{
			//		vm.ItemDisplayVms.Remove(item);
			//	}

			//	tempItems.Clear();
			//}
			//else
			//{
			//	if (vm.SelectedItem == null)
			//		return;

			//	var messageBox = MessageBox.Show("Are you sure you want to remove " +
			//		vm.SelectedItem.DisplayName, "Remove Item", MessageBoxButton.YesNo);

			//	if (messageBox == MessageBoxResult.No)
			//		return;

			//	vm.Inventory.Remove(vm.SelectedItem.BoundItem);
			//	vm.ItemDisplayVms.Remove(vm.SelectedItem);
			//}

			//if (vm.ItemDisplayVms.Count < 1) return;
			//vm.SelectedItem = vm.ItemDisplayVms[0];
		}
	}
}
