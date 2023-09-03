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
		private readonly CharacterInventoryViewModel vm;

		public RemovePropertyFromItemCommand(CharacterInventoryViewModel _vm)
		{
			vm = _vm;
		}

		public override void Execute(object? parameter)
		{
			if (vm.SelectedItem == null || vm.PrevSelectedProperty == null)
				return;

			vm.SelectedItem.BoundItem.RemoveProperty(vm.PrevSelectedProperty.BoundProperty);
			vm.PropertiesToDisplay.Remove(vm.PrevSelectedProperty);
		}
	}
}
