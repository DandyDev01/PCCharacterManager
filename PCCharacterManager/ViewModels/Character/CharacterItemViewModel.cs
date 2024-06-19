using PCCharacterManager.Commands;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
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
		private string _characterPath;
		public string CharacterPath => _characterPath;

		private string _characterName;
		public string CharacterName => _characterName;

		private string _id;
		public string Id => _id;

		private string _characterClass;
		public string CharacterClass => _characterClass;

		private string _characterLevel;
		public string CharacterLevel => _characterLevel;

		private string _characterDateModified;
		public string CharacterDateModified => _characterDateModified;

		private string _characterRace;
		public string CharacterRace => _characterRace;

		private CharacterType _characterType;
		public CharacterType CharacterType => _characterType;

		public SelectCharacterCommand SelectCharacterCommand { get; private set; }
		public ICommand DeleteCharacterCommand { get; private set; }

		public Action<string>? DeleteAction;

		//TODO: get the characterStore from BookVM
		public CharacterItemViewModel(CharacterStore characterStore, DnD5eCharacter character, string characterPath, 
			DialogServiceBase dialogService)
		{
			_characterName = character.Name;
			_characterClass = character.CharacterClass.Name;
			_characterLevel = character.Level.Level.ToString();
			_characterDateModified = character.DateModified;
			_characterPath = characterPath;
			_characterType = character.CharacterType;
			_characterRace = character.Race.Name;
			_id = character.Id;

			SelectCharacterCommand = new SelectCharacterCommand(characterStore, _characterPath, dialogService);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
		}

		public void Update(DnD5eCharacter character)
		{
			_characterName = character.Name;
			_characterClass = character.CharacterClass.Name;
			_characterLevel = character.Level.Level.ToString();
			_characterDateModified = character.DateModified;
			_characterType = character.CharacterType;
			_characterRace = character.Race.Name;

			_characterPath = CharacterTypeHelper.BuildPath(character);

			SelectCharacterCommand._characterPath = _characterPath;

			OnPropertyChanged(nameof(_characterName));
			OnPropertyChanged(nameof(_characterClass));
			OnPropertyChanged(nameof(_characterLevel));
			OnPropertyChanged(nameof(_characterDateModified));
			OnPropertyChanged(nameof(_characterType));
			OnPropertyChanged(nameof(_characterRace));
		}

		private void DeleteCharacter()
		{
			DeleteAction?.Invoke(_characterPath);
		}
	}
}
