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
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			NameSortCommand = new RelayCommand(NameSort);
			LevelSortCommand = new RelayCommand(LevelSort);
			ClassSortCommand = new RelayCommand(ClassSort);
			DataModifiedSortCommand = new RelayCommand(DataModifiedSort);
			CharacterTypeSortCommand = new RelayCommand(CharacterTypeSort);
			CharacterRaceSortCommand = new RelayCommand(CharacterRaceSort);

			characterStore.CharacterCreate += LoadCharacter;
			CharacterItems.OrderBy(x => x.CharacterDateModified).First().SelectCharacterCommand.Execute(null);
		}

		/// <summary>
		/// sorts the characters displayed by their race
		/// </summary>
		private void CharacterRaceSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterRace));
		}

		/// <summary>
		/// sorts the characters displayed by modified data
		/// </summary>
		private void DataModifiedSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterDateModified));
		}

		/// <summary>
		/// sorts the characters displayed by their class
		/// </summary>
		private void ClassSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterClass));
		}

		/// <summary>
		/// sorts the characters displayed by their levels
		/// </summary>
		private void LevelSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterLevel));
		}

		/// <summary>
		/// sorts the characters displayed by their names
		/// </summary>
		public void NameSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterName));
		}

		/// <summary>
		/// sorts the characters displayed by their character type
		/// </summary>
		private void CharacterTypeSort()
		{
			collectionViewPropertySort.Sort(nameof(CharacterItemViewModel.CharacterType));
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
			CharacterItemViewModel characterItem = CharacterItems.First(c => c.CharacterName == characterStore.SelectedCharacter.Name);
			characterItem.Update(characterStore.SelectedCharacter);
		}
		
		/// <summary>
		/// Delete the selected character for the database and select a new one.
		/// Opens character creator if there are no more characters in the database.
		/// </summary>
		public void DeleteCharacter()
		{
			if (characterStore.SelectedCharacter == null) return;

			var results = MessageBox.Show("are you sure you want to delete the character " + characterStore.SelectedCharacter.Name + "?", 
				"Delete Character", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (results == MessageBoxResult.No) return;

			DnD5eCharacter character = characterStore.SelectedCharacter;
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

			dataService.Delete(characterStore.SelectedCharacter);

			if(CharacterItems.Count <= 0)
			{
				CreateCharacterWindow();
				return;
			}


			CharacterItems[0].SelectCharacterCommand?.Execute(null);
			CharacterItems.Remove(item);
		}


		/// <summary>
		/// Saves the selected character to the database
		/// </summary>
		public void SaveCharacter()
		{
			dataService.Save(characterStore.SelectedCharacter);
			Update();
		}


	} // end class
}
