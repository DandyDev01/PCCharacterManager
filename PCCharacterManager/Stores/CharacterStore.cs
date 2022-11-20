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
		public Character? SelectedCharacter { get; private set; }
		public event Action<Character>? CharacterCreate;
		public event Action<Character>? SelectedCharacterChange;

		public void CreateCharacter(Character character)
		{
			CharacterCreate?.Invoke(character);
		}

		/// <summary>
		/// binds the selected character to a specified character
		/// </summary>
		/// <param name="characterToBind">character to bind to</param>
		public void BindSelectedCharacter(Character characterToBind)
		{
			SelectedCharacterChange?.Invoke(characterToBind);
			SelectedCharacter = characterToBind;
		}
	}
}
