using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCCharacterManager.ViewModels;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Models;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class AddItemToInventoryCommand : BaseCommand
	{
		private readonly CharacterInventoryViewModel vm;

		public AddItemToInventoryCommand(CharacterInventoryViewModel _vm)
		{
			vm = _vm;
		}

		public override void Execute(object parameter)
		{
			Window window = new AddItemDialogWindow();
			DialogWindowAddItemViewModel dialogContext =
				new DialogWindowAddItemViewModel(vm.DataService, vm.CharacterStore, window, vm.SelectedCharacter);
			window.DataContext = dialogContext;

			window.ShowDialog();

			if (window.DialogResult == false) return;

			Item selectedItem = dialogContext.SelectedItem.BoundItem;
			ItemDisplayViewModel displayVM = new ItemDisplayViewModel(selectedItem);
			vm.SelectedCharacter.Inventory.Add(selectedItem);
			vm.ItemDisplayVms.Add(displayVM);
		}
	}
}
