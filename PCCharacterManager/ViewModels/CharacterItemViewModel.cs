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
		private Character boundCharacter;
		public Character BoundCharacter
		{
			get { return boundCharacter; }
			set { OnPropertyChaged(ref boundCharacter, value); }
		}

		public ICommand SelectCharacterCommand { get; private set; }

		//TODO: get the characterStore from BookVM
		public CharacterItemViewModel(CharacterStore characterStore, Character character)
		{
			boundCharacter = character;

			SelectCharacterCommand = new SelectCharacterCommand(characterStore, this,
				boundCharacter);
		}
	}
}
