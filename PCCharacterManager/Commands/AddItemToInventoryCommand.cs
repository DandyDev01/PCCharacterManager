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
		private readonly CharacterInventoryViewModel _characterInventoryViewModel;
		private readonly DialogServiceBase _dialogService;

		public AddItemToInventoryCommand(CharacterInventoryViewModel characterInventoryViewModel, DialogServiceBase dialogService)
		{
			_characterInventoryViewModel = characterInventoryViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			DialogWindowAddItemViewModel dialogContext = new DialogWindowAddItemViewModel(_dialogService);
			string result = string.Empty;
			_dialogService.ShowDialog<AddItemDialogWindow, DialogWindowAddItemViewModel>(dialogContext, r =>
			{
				result = r;
			});

			if (result == false.ToString() || dialogContext.InventoryVM.SelectedItem == null) 
				return;

			if (dialogContext.InventoryVM.SelectedItem.BoundItem is not Item item)
				return;

			Item selectedItem = item;
			ItemViewModel displayVM = new ItemViewModel(selectedItem);
			_characterInventoryViewModel.Inventory.Add(selectedItem);
			_characterInventoryViewModel.ItemDisplayVms.Add(displayVM);
			_characterInventoryViewModel.CalculateInventoryWeight();
		}
	}
}
