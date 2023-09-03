using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class LevelCharacterCommand : BaseCommand
	{
		private readonly CharacterStore characterStore;

		public LevelCharacterCommand(CharacterStore _characterStore)
		{
			characterStore = _characterStore;
		}

		public override void Execute(object? parameter)
		{
			if (characterStore.SelectedCharacter == null)
				return;

			CharacterLeveler leveler = new();

			leveler.LevelCharacter(characterStore.SelectedCharacter);

			//OnPropertyChaged("characterStore.SelectedCharacter");
		}
	}
}
