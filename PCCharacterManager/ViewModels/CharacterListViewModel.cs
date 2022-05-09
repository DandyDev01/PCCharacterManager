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
		private ICharacterDataService dataService;

		private readonly CharacterStore characterStore;

		private CharacterItemViewModel characterItemVM;
		public CharacterItemViewModel CharacterItemVM
		{
			get { return characterItemVM; }
			set { OnPropertyChaged(ref characterItemVM, value); }
		}


		// ObservableCollection is a wpf friendly list
		public static ObservableCollection<Character> Characters { get; private set; }
		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }

		public CharacterListViewModel(ICharacterDataService dataService, CharacterStore _characterStore)
		{

			characterStore = _characterStore;
			this.dataService = dataService;

			LoadCharacters();

			characterStore.CharacterCreate += LoadCharacters;

		}

		public void LoadCharacters(Character character = null)
		{

			Characters = new ObservableCollection<Character>(dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();

			Character[] characterArray = Characters.ToArray();


			for (int i = 0; i < Characters.Count; i++)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, characterArray[i]));
			}

			if (Characters.Count > 0)
				characterStore.CharacterChange(Characters[0]);


			OnPropertyChaged("CharacterItems");
		}
	}
}
