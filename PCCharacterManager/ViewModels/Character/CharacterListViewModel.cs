using PCCharacterManager.Commands;
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
	/// <summary>
	/// this class is responsible for the logic of displaying characters in the database to a list
	/// </summary>
	public class CharacterListViewModel : ObservableObject
	{
		private readonly CharacterStore characterStore;
		private readonly ICharacterDataService dataService;
		private readonly CollectionViewPropertySort collectionViewPropertySort;

		public ObservableCollection<CharacterItemViewModel> CharacterItems { get; private set; }
		public ICollectionView CharacterCollectionView { get; }

		public ICommand CreateCharacterCommand {get;}
		public ICommand DeleteCharacterCommand {get;}
		public ICommand NameSortCommand { get; }
		public ICommand LevelSortCommand { get; }
		public ICommand ClassSortCommand { get; }
		public ICommand DataModifiedSortCommand { get; }
		public ICommand CharacterTypeSortCommand { get; }
		public ICommand CharacterRaceSortCommand { get; }

		public CharacterListViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
		{
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			characterStore = _characterStore;
			dataService = _dataService;

			while (_dataService.GetCharacters().Count() < 1)
			{
				CreateCharacterWindow();
			}

			List<DnD5eCharacter> characters = new List<DnD5eCharacter>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();
			CharacterCollectionView = CollectionViewSource.GetDefaultView(CharacterItems);
			collectionViewPropertySort = new CollectionViewPropertySort(CharacterCollectionView);
			CharacterCollectionView.SortDescriptions.Add(new SortDescription(nameof(CharacterItemViewModel.CharacterDateModified), ListSortDirection.Descending));

			List<string> characterPaths = _dataService.GetCharacterFilePaths().ToList();
			for (int i = 0; i < characters.Count; i++)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, characters[i], characterPaths[i]));
			}

			CreateCharacterCommand = new RelayCommand(CreateCharacterWindow);

			NameSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterName));
			LevelSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(CharacterItemViewModel.CharacterLevel));
			ClassSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterClass));
			DataModifiedSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterDateModified));
			CharacterTypeSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterType));
			CharacterRaceSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterRace));

			characterStore.CharacterCreate += LoadCharacter;
			CharacterItems.OrderBy(x => x.CharacterDateModified).First().SelectCharacterCommand.Execute(null);
		}

		/// <summary>
		/// opens a dialog window to create a new character
		/// </summary>
		public void CreateCharacterWindow()
		{
			Window newCharacterWindow = new CreateCharacterDialogWindow();
			newCharacterWindow.DataContext = new DialogWindowCharacterCreaterViewModel(characterStore, newCharacterWindow);

			newCharacterWindow.ShowDialog();
		}

		/// <summary>
		/// creates a CharacterItemViewModel for the given character and adds them to the list of
		/// characters to display
		/// </summary>
		/// <param name="_character">character to add</param>
		private void LoadCharacter(DnD5eCharacter _character)
		{
			if (_character == null)
				return;

			if(_character is StarfinderCharacter starfinder)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, _character,
					StarfinderResources.CharacterDataDir + "/" + _character.Name + ".json"));
			}
			else if(_character is DnD5eCharacter)
			{
				CharacterItems.Add(new CharacterItemViewModel(characterStore, _character, 
					DnD5eResources.CharacterDataDir + "/" + _character.Name + ".json"));
			}

		}

		/// <summary>
		/// Updates the characterItem of the selected character
		/// </summary>
		private void Update()
		{
			if (characterStore.SelectedCharacter == null)
				return;


			CharacterItemViewModel? characterItem = CharacterItems.FirstOrDefault(c => c.CharacterName == characterStore.SelectedCharacter.Name);

			if (characterItem == null)
				return;

			characterItem.Update(characterStore.SelectedCharacter);
		}
		
		/// <summary>
		/// Delete the selected character for the database and select a new one.
		/// Opens character creator if there are no more characters in the database.
		/// </summary>
		public void DeleteCharacter()
		{
			if (characterStore.SelectedCharacter == null) 
				return;

			var results = MessageBox.Show("are you sure you want to delete the character " + characterStore.SelectedCharacter.Name + "?", 
				"Delete Character", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (results == MessageBoxResult.No) 
				return;

			DnD5eCharacter character = characterStore.SelectedCharacter;
			CharacterItemViewModel? item = CharacterItems.First(x => x.CharacterName == character.Name);
		
			if(item == null)
			{
				MessageBox.Show("no character with name " + characterStore.SelectedCharacter.Name + " exists", "Could not find Character", 
					MessageBoxButton.OK, MessageBoxImage.Error);
				
				return;
			}

			dataService.Delete(characterStore.SelectedCharacter);

			if(CharacterItems.Count <= 0)
			{
				CreateCharacterWindow();
				return;
			}

			CharacterItems.Remove(item);
			CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}


		/// <summary>
		/// Saves the selected character to the database
		/// </summary>
		public void SaveCharacter()
		{
			if (characterStore.SelectedCharacter == null)
				return;

			dataService.Save(characterStore.SelectedCharacter);
			Update();
		}


	} // end class
}
