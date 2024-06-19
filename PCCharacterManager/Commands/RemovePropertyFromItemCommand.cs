using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class RemovePropertyFromItemCommand : BaseCommand
	{
		private readonly CharacterInventoryViewModel _characterInventoryViewModel;

		public RemovePropertyFromItemCommand(CharacterInventoryViewModel characterInventoryViewModel)
		{
			_characterInventoryViewModel = characterInventoryViewModel;
		}

		public override void Execute(object? parameter)
		{
			if (_characterInventoryViewModel.SelectedItem == null || _characterInventoryViewModel.PrevSelectedProperty == null)
				return;

			_characterInventoryViewModel.SelectedItem.BoundItem.RemoveProperty(_characterInventoryViewModel.PrevSelectedProperty.BoundProperty);
			_characterInventoryViewModel.PropertiesToDisplay.Remove(_characterInventoryViewModel.PrevSelectedProperty);
		}
	}
}
