using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterStatsViewModel : ObservableObject
	{
		private readonly CharacterTypeHelper _characterTypeHelper;

		private DnD5eCharacter _selectedCharacter;
		public DnD5eCharacter SelectedCharacter 
		{ 
			get { return _selectedCharacter; }
			set { OnPropertyChanged(ref _selectedCharacter, value); }
		}

		public CharacterInfoViewModel CharacterInfoViewModel { get; }
		public StarfinderCharacterInfoViewModel StarfinderCharacterInfoViewModel { get; }
		public DarkSoulsCharacterInfoViewModel DarkSoulsCharacterInfoViewModel { get; }

		public StarfinderAbilitiesAndSkillsViewModel StarfinderAbilitiesAndSkillsVM { get; }
		public CharacterAbilitiesViewModel CharacterAbilitiesViewModel { get; }

		public CharacterTypeHelper CharacterTypeHelper => _characterTypeHelper;
		
		public CharacterStatsViewModel(CharacterStore characterStore, DialogServiceBase dialogService, RecoveryBase recovery)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_characterTypeHelper = new CharacterTypeHelper();
			_selectedCharacter = characterStore.SelectedCharacter;

			CharacterInfoViewModel = new CharacterInfoViewModel(characterStore, dialogService, recovery);
			CharacterAbilitiesViewModel = new CharacterAbilitiesViewModel(characterStore);

			DarkSoulsCharacterInfoViewModel = new DarkSoulsCharacterInfoViewModel(characterStore, dialogService, recovery);

			StarfinderCharacterInfoViewModel = new StarfinderCharacterInfoViewModel(characterStore, dialogService, recovery);
			StarfinderAbilitiesAndSkillsVM = new StarfinderAbilitiesAndSkillsViewModel(characterStore);

			_characterTypeHelper.SetCharacterTypeFlags(_selectedCharacter.CharacterType);
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			_characterTypeHelper.SetCharacterTypeFlags(newCharacter.CharacterType);
		}
	}
}
