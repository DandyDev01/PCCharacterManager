using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterListViewModel : ObservableObject
	{
		private readonly CharacterStore characterStore;
		private readonly ICharacterDataService dataService;
		private readonly CollectionViewPropertySort collectionViewPropertySort;
		private readonly UpdateHandler updateHandler;

		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }
		public ICollectionView CharacterCollectionView { get; }

		public ICommand CreateCharacterCommand {get;}
		public ICommand DeleteCharacterCommand {get;}
		public ICommand NameSortCommand { get; }
		public ICommand LevelSortCommand { get; }
		public ICommand ClassSortCommand { get; }
		public ICommand DataModifiedSortCommand { get; }

		public CharacterListViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
		{
			characterStore = _characterStore;
			dataService = _dataService;
			updateHandler = new UpdateHandler();

			while (_dataService.GetCharacters().Count() < 1)
			{
				CreateCharacterWindow();
			}

			//updateHandler.HandleCharacterFormatChanges(_dataService);
			
			List<Character> characters = new List<Character>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();
			CharacterCollectionView = CollectionViewSource.GetDefaultView(CharacterItems);
			collectionViewPropertySort = new CollectionViewPropertySort(CharacterCollectionView);
			CharacterCollectionView.SortDescriptions.Add(new SortDescription(nameof(CharacterItemViewModel.CharacterDateModified), ListSortDirection.Descending));

			string[] characterPaths = _dataService.GetCharacterFilePaths().ToArray();
			for (int i = 0; i < characters.Count; i++)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, characters[i], characterPaths[i]));
			}

			CreateCharacterCommand = new RelayCommand(CreateCharacterWindow);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			NameSortCommand = new RelayCommand(NameSort);
			LevelSortCommand = new RelayCommand(LevelSort);
			ClassSortCommand = new RelayCommand(ClassSort);
			DataModifiedSortCommand = new RelayCommand(DataModifiedSort);

			characterStore.CharacterCreate += LoadCharacters;			
		}

		private void DataModifiedSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterDateModified));
		}

		private void ClassSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterClass));
		}

		private void LevelSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterLevel));
		}

		public void NameSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterName));
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
