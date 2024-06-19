using PCCharacterManager.Models;
using PCCharacterManager.Models.Levelers;
using PCCharacterManager.Services;
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
		private readonly DarkSoulsCharacterLeveler _darkSoulsLeveler;
		private CharacterLeveler _leveler;

		public LevelCharacterCommand(CharacterStore characterStore, DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_dnd5eLeveler = new(dialogService);
			_starfinderLeveler = new(dialogService);
			_darkSoulsLeveler = new(dialogService);
			_leveler = _dnd5eLeveler;
		}

		public override void Execute(object? parameter)
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			_leveler = GetLeveler();

			_leveler.LevelCharacter(_characterStore.SelectedCharacter);

			_characterStore.LevelCharacter();
		}

		private CharacterLeveler GetLeveler()
		{
			switch (_characterStore.SelectedCharacter.CharacterType)
			{
				case CharacterType.DnD5e:
					return _dnd5eLeveler;
				case CharacterType.starfinder:
					return _starfinderLeveler;
				case CharacterType.dark_souls:
					return _darkSoulsLeveler;
				default:
					throw new Exception("Leveler does not exist for character type: " + 
						_characterStore.SelectedCharacter.CharacterType);
			}
		}
	}
}
