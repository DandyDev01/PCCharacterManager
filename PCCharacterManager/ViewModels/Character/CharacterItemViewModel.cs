using PCCharacterManager.Commands;
using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterItemViewModel : ObservableObject
	{
		private readonly string characterPath;
		public string CharacterPath => characterPath;

		private string characterName;
		public string CharacterName => characterName;

		private string characterClass;
		public string CharacterClass => characterClass;

		private string characterLevel;
		public string CharacterLevel => characterLevel;

		private string characterDateModified;
		public string CharacterDateModified => characterDateModified;

		private string characterRace;
		public string CharacterRace => characterRace;

		private CharacterType characterType;
		public CharacterType CharacterType => characterType;

		public ICommand SelectCharacterCommand { get; private set; }
		public ICommand DeleteCharacterCommand { get; private set; }

		public Action<string> DeletAction;

		//TODO: get the characterStore from BookVM
		public CharacterItemViewModel(CharacterStore characterStore, DnD5eCharacter character, string _characterPath)
		{
			characterName = character.Name;
			characterClass = character.CharacterClass.Name;
			characterLevel = character.Level.Level.ToString();
			characterDateModified = character.DateModified;
			characterPath = _characterPath;
			characterType = character.CharacterType;
			characterRace = character.Race.Name;

			SelectCharacterCommand = new SelectCharacterCommand(characterStore, characterPath);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
		}

		public void Update(DnD5eCharacter character)
		{
			characterName = character.Name;
			characterClass = character.CharacterClass.Name;
			characterLevel = character.Level.Level.ToString();
			characterDateModified = character.DateModified;
			characterType = character.CharacterType;
			characterRace = character.Race.Name;

			OnPropertyChanged(nameof(characterName));
			OnPropertyChanged(nameof(characterClass));
			OnPropertyChanged(nameof(characterLevel));
			OnPropertyChanged(nameof(characterDateModified));
			OnPropertyChanged(nameof(characterType));
			OnPropertyChanged(nameof(characterRace));
		}

		private void DeleteCharacter()
		{
			DeletAction?.Invoke(characterPath);
		}
	}
}
