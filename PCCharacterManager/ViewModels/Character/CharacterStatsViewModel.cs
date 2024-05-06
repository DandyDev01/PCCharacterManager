using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public CharacterStatsViewModel(CharacterStore characterStore, DialogService dialogService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_selectedCharacter = characterStore.SelectedCharacter;

			CharacterInfoViewModel = new CharacterInfoViewModel(characterStore, dialogService);
			StarfinderCharacterInfoViewModel = new StarfinderCharacterInfoViewModel(characterStore, dialogService); 
			StarfinderAbilitiesAndSkillsVM = new StarfinderAbilitiesAndSkillsViewModel(characterStore);
			CharacterAbilitiesViewModel = new CharacterAbilitiesViewModel(characterStore);

			if (_selectedCharacter is StarfinderCharacter)
			{
				Is5e = false;
				IsStarfinder = true;
			}
			else if (_selectedCharacter is not null)
			{
				Is5e = true;
				IsStarfinder = false;
			}
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			if(_selectedCharacter is StarfinderCharacter)
			{
				Is5e = false;
				IsStarfinder = true;
			}
			else if (_selectedCharacter is DnD5eCharacter)
			{
				Is5e = true;
				IsStarfinder = false;
			}
		}
	}
}
