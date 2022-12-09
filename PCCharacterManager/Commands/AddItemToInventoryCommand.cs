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

			if (dialogContext.SelectedItem == null) return;

			Item selectedItem = dialogContext.SelectedItem.BoundItem;
			ItemDisplayViewModel displayVM = new ItemDisplayViewModel(selectedItem);
			vm.SelectedCharacter.Inventory.Add(selectedItem);
			vm.ItemDisplayVms.Add(displayVM);
			//vm.ItemsToShow.Add(displayVM);
			//vm.FilteredItems[selectedItem.Tag].Add(displayVM);
			//vm.ItemsToShow.OrderBy(x => x.DisplayName);
		}
	}
}
