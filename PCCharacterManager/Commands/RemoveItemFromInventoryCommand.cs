using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
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
			if (vm.SelectedItem == null)
				return;

			var messageBox = MessageBox.Show("Are you sure you want to remove " + vm.SelectedItem.DisplayName, "Remove Item", MessageBoxButton.YesNo);

			if (messageBox == MessageBoxResult.No)
				return;

			vm.SelectedCharacter.Inventory.Remove(vm.SelectedItem.BoundItem);
			vm.ItemsToShow.Remove(vm.SelectedItem);
			vm.SelectedItem = vm.ItemsToShow[0];
		}
	}
}
