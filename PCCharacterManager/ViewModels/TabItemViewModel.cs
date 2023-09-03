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
		protected DnD5eCharacter? selectedCharacter;
		public DnD5eCharacter? SelectedCharacter
		{
			get { return selectedCharacter; }
			set { OnPropertyChanged(ref selectedCharacter, value); }
		}
		
		protected readonly CharacterStore characterStore;
		protected readonly ICharacterDataService dataService;

		public CharacterStore CharacterStore { get { return characterStore; } }
		public ICharacterDataService DataService { get { return dataService; } }

		public TabItemViewModel(CharacterStore _characterStore, ICharacterDataService _dataService,
			DnD5eCharacter? _selectedCharacter = null)
		{
			characterStore = _characterStore;
			dataService = _dataService;
			selectedCharacter = _selectedCharacter;
		}

		protected virtual void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;
		}
	}
}
