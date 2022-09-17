using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Stores
{
	public class CharacterStore
	{
		public Character SelectedCharacter { get; private set; }
		public event Action<Character> CharacterCreate;
		public event Action<Character> SelectedCharacterChange;

		public void CreateCharacter(Character character)
		{
			CharacterCreate?.Invoke(character);
		}

		public void CharacterChange(Character newCharacter)
		{
			SelectedCharacterChange?.Invoke(newCharacter);
			SelectedCharacter = newCharacter;
			// NOTE: this is gross...find better way
			// this is here because it is the fastest way to set the pointer

		}

		public void SetSelectedCharacter(Character character)
		{
			SelectedCharacterChange?.Invoke(character);
			SelectedCharacter = character;
		}
	}
}
