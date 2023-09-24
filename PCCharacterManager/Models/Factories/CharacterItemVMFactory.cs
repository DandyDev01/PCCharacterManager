using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models.Factories
{
	public static class CharacterItemVMFactory
	{
		public static CharacterItemViewModel Create(DnD5eCharacter _character, CharacterStore _characterStore)
		{
			if (_character == null)
				throw new Exception("null value exeption");

			if (_character is StarfinderCharacter starfinder)
			{
				return new CharacterItemViewModel(_characterStore, _character,
					StarfinderResources.CharacterDataDir + "/" + _character.Name + ".json");
			}
			else if (_character is DnD5eCharacter)
			{
				return new CharacterItemViewModel(_characterStore, _character,
					DnD5eResources.CharacterDataDir + "/" + _character.Name + ".json");
			}

			throw new Exception("Fail");
		}
	}
}
