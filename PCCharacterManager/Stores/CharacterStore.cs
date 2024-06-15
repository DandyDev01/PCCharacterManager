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

		public DnD5eCharacter SelectedCharacter { get; private set; }

		public Action<DnD5eCharacter>? SaveSelectedCharacterOnChange { get; internal set; }
		public event Action<DnD5eCharacter>? CharacterCreate;
		public event Action<DnD5eCharacter>? SelectedCharacterChange;
		public event Action<DnD5eCharacter>? OnCharacterLevelup;

		public CharacterStore(RecoveryBase recovery)
		{ 
			SelectedCharacter = DnD5eCharacter.Default;
			_recovery = recovery;
		}

		public void CreateCharacter(DnD5eCharacter character)
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
		public void BindSelectedCharacter(DnD5eCharacter characterToBind)
		{
			if (SelectedCharacter is not null)
			{
				SelectedCharacter.OnCharacterChangedAction -= _recovery.RegisterChange;
			}
			else
			{
				return;
			}

			string oldID = SelectedCharacter.Id;

			SaveSelectedCharacterOnChange?.Invoke(SelectedCharacter);
			SelectedCharacter = characterToBind;

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
