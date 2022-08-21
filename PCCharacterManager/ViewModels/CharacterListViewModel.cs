using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class CharacterListViewModel : ObservableObject
	{
		private readonly CharacterStore characterStore;
		private readonly UpdateHandler updateHandler;

		public ObservableCollection<Character> Characters { get; private set; }
		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }

		public CharacterListViewModel(ICharacterDataService _dataService, CharacterStore _characterStore)
		{
			characterStore = _characterStore;
			updateHandler = new UpdateHandler();
			//updateHandler.HandleCharacterFormatChanges(_dataService);
			
			Characters = new ObservableCollection<Character>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();

			foreach (var character in Characters)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, character));
			}

			characterStore.CharacterChange(Characters[0]);

			characterStore.CharacterCreate += LoadCharacters;
		}

		private void LoadCharacters(Character _character)
		{
			Characters.Add(_character);
			CharacterItems.Add(new CharacterItemViewModel(characterStore, _character));
		}
	} // end class
}
