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

		private bool _is5e;
		public bool Is5e
		{
			get
			{
				return _is5e;
			}
			set
			{
				OnPropertyChanged(ref _is5e, value);
			}
		}

		private bool _isStarfinder;
		public bool IsStarfinder
		{
			get
			{
				return _isStarfinder;
			}
			set
			{
				OnPropertyChanged(ref _isStarfinder, value);
			}
		}

		private bool _isDarkSouls;
		public bool IsDarkSouls
		{
			get
			{
				return _isDarkSouls;
			}
			set
			{
				OnPropertyChanged(ref _isDarkSouls, value);
			}
		}

		public CharacterStatsViewModel(CharacterStore characterStore, DialogServiceBase dialogService, RecoveryBase recovery)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_selectedCharacter = characterStore.SelectedCharacter;

			CharacterInfoViewModel = new CharacterInfoViewModel(characterStore, dialogService, recovery);
			CharacterAbilitiesViewModel = new CharacterAbilitiesViewModel(characterStore);

			DarkSoulsCharacterInfoViewModel = new DarkSoulsCharacterInfoViewModel(characterStore, dialogService, recovery);

			StarfinderCharacterInfoViewModel = new StarfinderCharacterInfoViewModel(characterStore, dialogService, recovery);
			StarfinderAbilitiesAndSkillsVM = new StarfinderAbilitiesAndSkillsViewModel(characterStore);
			
			SetCharacterType();
		}


		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			SetCharacterType();
		}
		
		/// <summary>
		/// Updates the flags for character types
		/// </summary>
		private void SetCharacterType()
		{
			if (_selectedCharacter is StarfinderCharacter)
			{
				Is5e = false;
				IsDarkSouls = false;
				IsStarfinder = true;
			}
			else if (_selectedCharacter is DarkSoulsCharacter)
			{
				Is5e = false;
				IsDarkSouls = true;
				IsStarfinder = false;
			}
			else if (_selectedCharacter is not null)
			{
				Is5e = true;
				IsDarkSouls = false;
				IsStarfinder = false;
			}
		}
	}
}
