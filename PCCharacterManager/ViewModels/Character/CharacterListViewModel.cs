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
		private readonly CharacterStore _characterStore;
		private readonly ICharacterDataService _dataService;
		private readonly CollectionViewPropertySort _collectionViewPropertySort;

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

		public CharacterListViewModel(CharacterStore characterStore, ICharacterDataService dataService,
			DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_dataService = dataService;

			CreateCharacterCommand = new CreateCharacterCommand(characterStore, dialogService);
			DeleteCharacterCommand = new DeleteCharacterCommand(this, dataService, characterStore);

			_characterStore.CharacterCreate += LoadCharacter;
			_dataService.OnSave += Update;

			List<DnD5eCharacter> characters = new(dataService.GetCharacters());

			CharacterItems = new ObservableCollection<CharacterItemViewModel>();
			CharacterCollectionView = CollectionViewSource.GetDefaultView(CharacterItems);
			_collectionViewPropertySort = new CollectionViewPropertySort(CharacterCollectionView);
			CharacterCollectionView.SortDescriptions.Add(new SortDescription(nameof(CharacterItemViewModel.CharacterDateModified), ListSortDirection.Descending));

			while (!dataService.GetCharacters().Any())
			{
				CreateCharacterCommand.Execute(null);
			}

			List<string> characterPaths = dataService.GetCharacterFilePaths().ToList();
			for (int i = 0; i < characters.Count; i++)
			{
				CharacterItemViewModel characterItemVM = new(this._characterStore, characters[i], characterPaths[i]);
				characterItemVM.DeleteAction += DeleteCharacter;

				CharacterItems.Add(characterItemVM);
			}

			NameSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterName));
			LevelSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(CharacterItemViewModel.CharacterLevel));
			ClassSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterClass));
			DataModifiedSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterDateModified));
			CharacterTypeSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterType));
			CharacterRaceSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort, 
				nameof(CharacterItemViewModel.CharacterRace));

			CharacterItems.OrderBy(x => x.CharacterDateModified).First().SelectCharacterCommand.Execute(null);
		}

		private void DeleteCharacter(string path)
		{
			_dataService.Delete(path);

			CharacterItemViewModel? characterItemVM 
				= CharacterItems.Where(c => path.Contains(c.CharacterName)).FirstOrDefault();

			if (characterItemVM == null)
				return;

			characterItemVM.DeleteAction -= DeleteCharacter;

			CharacterItems.Remove(characterItemVM);
			CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}

		/// <summary>
		/// creates a CharacterItemViewModel for the given character and adds them to the list of
		/// characters to display
		/// </summary>
		/// <param name="character">character to add</param>
		private void LoadCharacter(DnD5eCharacter character)
		{
			if (character == null)
				return;

			CharacterItemViewModel characterItemVM = CharacterItemVMFactory.Create(character, _characterStore);
			characterItemVM.DeleteAction += DeleteCharacter;

			CharacterItems.Add(characterItemVM);
		}

		/// <summary>
		/// Updates the characterItem of the selected character
		/// </summary>
		private void Update()
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			CharacterItemViewModel? characterItem = CharacterItems.FirstOrDefault(c => c.Id == _characterStore.SelectedCharacter.Id);

			if (characterItem == null)
				return;

			characterItem.Update(_characterStore.SelectedCharacter);
		}
		
		/// <summary>
		/// Saves the selected character to the database
		/// </summary>
		public void SaveCharacter()
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			_dataService.Save(_characterStore.SelectedCharacter); 
			Update();
			CharacterCollectionView?.Refresh();
		}

	} // end class
}
