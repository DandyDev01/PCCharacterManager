using PCCharacterManager.Services;
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
		public static CharacterItemViewModel Create(DnD5eCharacter character, CharacterStore characterStore, 
			DialogServiceBase dialogService)
		{
			if (character == null)
				throw new Exception("null value exeption");

			if (character is StarfinderCharacter starfinder)
			{
				return new CharacterItemViewModel(characterStore, character,
					StarfinderResources.CharacterDataDir + "/" + character.Name + character.Id + ".json", dialogService);
			}
			else if (character is DnD5eCharacter)
			{
				return new CharacterItemViewModel(characterStore, character,
					DnD5eResources.CharacterDataDir + "/" + character.Name + character.Id + ".json", dialogService);
			}

			throw new Exception("Fail");
		}
	}
}
