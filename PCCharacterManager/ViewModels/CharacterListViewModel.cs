using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterListViewModel : ObservableObject
	{
		private readonly CharacterStore characterStore;
		private readonly ICharacterDataService dataService;
		private readonly UpdateHandler updateHandler;

		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }

		public ICommand CreateCharacterCommand {get;}
		public ICommand DeleteCharacterCommand {get;}

		public CharacterListViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
		{
			characterStore = _characterStore;
			dataService = _dataService;
			updateHandler = new UpdateHandler();

			while (_dataService.GetCharacters().Count() < 1)
			{
				CreateCharacterWindow();
			}

			CreateCharacterCommand = new RelayCommand(CreateCharacterWindow);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			//updateHandler.HandleCharacterFormatChanges(_dataService);
			
			List<Character> characters = new List<Character>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();

			string[] characterPaths = _dataService.GetCharacterFilePaths().ToArray();
			for (int i = 0; i < characters.Count; i++)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, characters[i], characterPaths[i]));
			}

			CharacterItems[0].SelectCharacterCommand.Execute(null);
			if (characters.Count > 0) characterStore.BindSelectedCharacter(characters[0]);

			characterStore.CharacterCreate += LoadCharacters;
			
		}

		public void CreateCharacterWindow()
		{
			Window newCharacterWindow = new CreateCharacterDialogWindow();
			newCharacterWindow.DataContext = new DialogWindowCharacterCreaterViewModel(dataService, characterStore, newCharacterWindow);

			newCharacterWindow.ShowDialog();
		}

		public void DeleteCharacter()
		{
			if (characterStore.SelectedCharacter == null) return;

			var results = MessageBox.Show("are you sure you want to delete the character " + characterStore.SelectedCharacter.Name + "?", 
				"Delete Character", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (results == MessageBoxResult.No) return;

			Character character = characterStore.SelectedCharacter;
			CharacterItemViewModel? item = null;
			foreach (CharacterItemViewModel _item in CharacterItems)
			{
				if (_item.CharacterName == character.Name)
				{
					item = _item;
					break;
				}
			}

			if(item == null)
			{
				MessageBox.Show("no character with name " + characterStore.SelectedCharacter.Name + " exists", "Could not find Character", 
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			CharacterItems.Remove(item);
			dataService.Delete(characterStore.SelectedCharacter);

			if(CharacterItems.Count <= 0)
			{
				CreateCharacterWindow();
				return;
			}


			CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}

		private void LoadCharacters(Character _character)
		{
			CharacterItems.Add(new CharacterItemViewModel(characterStore, _character, Resources.CharacterDataDir + "/" + _character.Name + ".json"));
		}
	} // end class
}
