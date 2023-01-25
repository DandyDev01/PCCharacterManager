using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterInfoViewModel : ObservableObject
	{
		private StarfinderCharacter starfinderCharacter;
		public StarfinderCharacter SelectedCharacter
		{
			get
			{
				return starfinderCharacter;
			}
			set
			{
				OnPropertyChanged(ref starfinderCharacter, value);
			}
		}

		public StarfinderCharacterInfoViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChange;
		}

		private void OnCharacterChange(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter as StarfinderCharacter;
		}
	}
}
