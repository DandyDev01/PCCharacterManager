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
		private readonly CharacterInventoryViewModel _characterInventoryViewModel;

		public AddPropertyToItemCommand(CharacterInventoryViewModel characterInventoryViewModel)
		{
			_characterInventoryViewModel = characterInventoryViewModel;
		}

		public override void Execute(object? parameter)
		{
			if (_characterInventoryViewModel.SelectedItem == null || _characterInventoryViewModel.SelectedItem.BoundItem == null)
				return;

			_characterInventoryViewModel.SelectedItem.BoundItem.AddProperty(new Property("name", "desc"));
			_characterInventoryViewModel.PropertiesToDisplay.Clear();
			foreach (var property in _characterInventoryViewModel.SelectedItem.BoundItem.Properties)
			{
				_characterInventoryViewModel.PropertiesToDisplay.Add(new PropertyEditableViewModel(property));
			}
		}
	}
}
