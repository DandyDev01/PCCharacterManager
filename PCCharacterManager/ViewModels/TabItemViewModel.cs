using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class TabItemViewModel : ObservableObject
	{
		protected DnD5eCharacter? _selectedCharacter;
		public DnD5eCharacter? SelectedCharacter
		{
			get { return _selectedCharacter; }
			set { OnPropertyChanged(ref _selectedCharacter, value); }
		}
		
		protected readonly CharacterStore _characterStore;
		protected readonly ICharacterDataService _dataService;

		public CharacterStore CharacterStore { get { return _characterStore; } }
		public ICharacterDataService DataService { get { return _dataService; } }

		public TabItemViewModel(CharacterStore characterStore, ICharacterDataService dataService,
			DnD5eCharacter? selectedCharacter = null)
		{
			_characterStore = characterStore;
			_dataService = dataService;
			_selectedCharacter = selectedCharacter;
		}

		protected virtual void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;
		}
	}
}
