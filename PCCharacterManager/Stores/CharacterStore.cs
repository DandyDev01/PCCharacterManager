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
		private readonly RecoveryBase _recovery;

		private CharacterBase _selectedCharacter;
		public CharacterBase SelectedCharacter => _selectedCharacter;

		public Action<CharacterBase>? SaveSelectedCharacterOnChange { get; internal set; }
		public event Action<CharacterBase>? CharacterCreate;
		public event Action<CharacterBase>? SelectedCharacterChange;
		public event Action<CharacterBase>? OnCharacterLevelup;

		public CharacterStore(RecoveryBase recovery)
		{ 
			_selectedCharacter = DnD5eCharacter.Default;
			_recovery = recovery;
		}

		public void CreateCharacter(CharacterBase character)
		{
			CharacterCreate?.Invoke(character);
		}

		public void LevelCharacter()
		{
			OnCharacterLevelup?.Invoke(SelectedCharacter);
		}

		/// <summary>
		/// binds the selected character to a specified character
		/// </summary>
		/// <param name="characterToBind">character to bind to</param>
		public void BindSelectedCharacter(CharacterBase characterToBind)
		{
			if (SelectedCharacter is not null)
			{
				SelectedCharacter.OnCharacterChangedAction -= _recovery.RegisterChange;
			}
			else
			{
				return;
			}

			if (characterToBind is null)
			{
				_selectedCharacter = null;
				return;
			}

			string oldID = SelectedCharacter.Id;

			SaveSelectedCharacterOnChange?.Invoke(_selectedCharacter);
			_selectedCharacter = characterToBind;

			SelectedCharacter.OnCharacterChangedAction += _recovery.RegisterChange;

			// only reset if the newly selected character is different from the currently select character.
			if (oldID != SelectedCharacter.Id) 
			{
				_recovery.ClearHistory();
				_recovery.RegisterChange(SelectedCharacter);
			}

			// causes bug that makes characters get saved when selected.
			// however, without this the displayed information is about the
			// previously selected character.
			SelectedCharacterChange?.Invoke(SelectedCharacter);
		}
	}
}
