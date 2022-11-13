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

		public ObservableCollection<Character> Characters { get; private set; }
		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }

		public ICommand CreateCharacterCommand {get;}
		public ICommand DeleteCharacterCommand {get;}

		public CharacterListViewModel(ICharacterDataService _dataService, CharacterStore _characterStore)
		{
			characterStore = _characterStore;
			dataService = _dataService;
			updateHandler = new UpdateHandler();

			CreateCharacterCommand = new RelayCommand(CreateCharacterWindow);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			//updateHandler.HandleCharacterFormatChanges(_dataService);
			
			Characters = new ObservableCollection<Character>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();

			foreach (var character in Characters)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, character));
			}

			if(Characters.Count > 0) characterStore.CharacterChange(Characters[0]);

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
				"Delete Character", MessageBoxButton.YesNo);

			if (results == MessageBoxResult.No) return;

			Character character = characterStore.SelectedCharacter;
			CharacterItemViewModel item = null;
			foreach (CharacterItemViewModel _item in CharacterItems)
			{
				if (_item.BoundCharacter == character)
				{
					item = _item;
					break;
				}
			}

			CharacterItems.Remove(item);
			Characters.Remove(character);
			dataService.Delete(characterStore.SelectedCharacter);
			characterStore.SetSelectedCharacter(Characters[0]);
		}

		private void LoadCharacters(Character _character)
		{
			Characters.Add(_character);
			CharacterItems.Add(new CharacterItemViewModel(characterStore, _character));
		}
	} // end class
}
