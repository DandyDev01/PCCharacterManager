using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Models.Factories;
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

			CreateCharacterCommand = new CreateCharacterCommand(_characterStore);
			DeleteCharacterCommand = new DeleteCharacterCommand(this, _dataService, _characterStore);

			while (_dataService.GetCharacters().Count() < 1)
			{
				CreateCharacterCommand.Execute(null);
			}

			List<DnD5eCharacter> characters = new List<DnD5eCharacter>(_dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();
			CharacterCollectionView = CollectionViewSource.GetDefaultView(CharacterItems);
			collectionViewPropertySort = new CollectionViewPropertySort(CharacterCollectionView);
			CharacterCollectionView.SortDescriptions.Add(new SortDescription(nameof(CharacterItemViewModel.CharacterDateModified), ListSortDirection.Descending));

			List<string> characterPaths = _dataService.GetCharacterFilePaths().ToList();
			for (int i = 0; i < characters.Count; i++)
			{
				CharacterItemViewModel characterItemVM = new CharacterItemViewModel(characterStore, characters[i], characterPaths[i]);
				characterItemVM.DeletAction += DeleteCharacter;

				CharacterItems.Add(characterItemVM);
			}

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

		private void DeleteCharacter(string path)
		{
			dataService.Delete(path);

			CharacterItemViewModel characterItemVM 
				= CharacterItems.Where(c => path.Contains(c.CharacterName)).FirstOrDefault();

			if (characterItemVM == null)
				return;

			CharacterItems.Remove(characterItemVM);
			CharacterItems[0].SelectCharacterCommand?.Execute(null);
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

			CharacterItemViewModel characterItemVM = CharacterItemVMFactory.Create(_character, characterStore);
			characterItemVM.DeletAction += DeleteCharacter;

			CharacterItems.Add(characterItemVM);
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
