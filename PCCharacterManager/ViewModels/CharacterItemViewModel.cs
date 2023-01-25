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

		public ICommand SelectCharacterCommand { get; private set; }

		//TODO: get the characterStore from BookVM
		public CharacterItemViewModel(CharacterStore characterStore, DnD5eCharacter character, string _characterPath)
		{
			characterName = character.Name;
			characterClass = character.CharacterClass.Name;
			characterLevel = character.Level.Level.ToString();
			characterDateModified = character.DateModified;
			characterPath = _characterPath;

			SelectCharacterCommand = new SelectCharacterCommand(characterStore, characterPath);
		}
	}
}
