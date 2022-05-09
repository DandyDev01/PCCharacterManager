using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class SelectCharacterCommand : BaseCommand
	{
		private readonly CharacterItemViewModel characterItemVM;
		private readonly CharacterStore characterStore;
		private readonly Character characterToSelect;

		public SelectCharacterCommand(CharacterStore characterStore,
			CharacterItemViewModel charaterItem, Character _characterToSelect)
		{
			this.characterStore = characterStore;
			characterItemVM = charaterItem;
			characterToSelect = _characterToSelect;
		}

		public override void Execute(object parameter)
		{
			//Console.WriteLine("selected character is: " + characterToSelect.Name);
			characterStore.CharacterChange(characterToSelect);

		}
	}
}
