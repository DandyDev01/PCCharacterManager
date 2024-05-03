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
		private readonly CharacterStore _characterStore;
		private readonly DnD5eCharacterLeveler _dnd5eLeveler;
		private readonly StarfinderCharacterLeveler _starfinderLeveler;
		private CharacterLeveler _leveler;

		public LevelCharacterCommand(CharacterStore characterStore)
		{
			_characterStore = characterStore;
			_dnd5eLeveler = new();
			_starfinderLeveler = new();
			_leveler = _dnd5eLeveler;
		}

		public override void Execute(object? parameter)
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			_leveler = _characterStore.SelectedCharacter is StarfinderCharacter ? _starfinderLeveler : _dnd5eLeveler;

			_leveler.LevelCharacter(_characterStore.SelectedCharacter);

			//OnPropertyChaged("characterStore.SelectedCharacter");
		}
	}
}
