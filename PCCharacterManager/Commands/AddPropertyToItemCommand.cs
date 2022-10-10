using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class AddPropertyToItemCommand : BaseCommand
	{
		private readonly CharacterInventoryViewModel vm;

		public AddPropertyToItemCommand(CharacterInventoryViewModel _vm)
		{
			vm = _vm;
		}

		public override void Execute(object parameter)
		{
			if (vm.SelectedItem == null)
				return;

			vm.SelectedItem.BoundItem.AddProperty(new Property("name", "desc"));
			vm.PropertiesToDisplay.Clear();
			foreach (var property in vm.SelectedItem.BoundItem.Properties)
			{
				vm.PropertiesToDisplay.Add(new PropertyEditableViewModel(property));
			}
		}
	}
}
