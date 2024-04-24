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
		private DnD5eCharacter selectedCharacter;
		public DnD5eCharacter SelectedCharacter 
		{ 
			get { return selectedCharacter; }
			set { OnPropertyChanged(ref selectedCharacter, value); }
		}

		public CharacterInfoViewModel CharacterInfoViewModel { get; }
		public StarfinderCharacterInfoViewModel StarfinderCharacterInfoViewModel { get; }
		public StarfinderAbilitiesAndSkillsViewModel StarfinderAbilitiesAndSkillsVM { get; }
		public CharacterAbilitiesViewModel CharacterAbilitiesViewModel { get; }

		private bool is5e;
		public bool Is5e
		{
			get
			{
				return is5e;
			}
			set
			{
				OnPropertyChanged(ref is5e, value);
			}
		}

		private bool isStarfinder;
		public bool IsStarfinder
		{
			get
			{
				return isStarfinder;
			}
			set
			{
				OnPropertyChanged(ref isStarfinder, value);
			}
		}

		public CharacterStatsViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			selectedCharacter = _characterStore.SelectedCharacter;

			CharacterInfoViewModel = new CharacterInfoViewModel(_characterStore);
			StarfinderCharacterInfoViewModel = new StarfinderCharacterInfoViewModel(_characterStore); 
			StarfinderAbilitiesAndSkillsVM = new StarfinderAbilitiesAndSkillsViewModel(_characterStore);
			CharacterAbilitiesViewModel = new CharacterAbilitiesViewModel(_characterStore);

			if (selectedCharacter is StarfinderCharacter)
			{
				Is5e = false;
				IsStarfinder = true;
			}
			else if (selectedCharacter is not null)
			{
				Is5e = true;
				IsStarfinder = false;
			}
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			if(selectedCharacter is StarfinderCharacter)
			{
				Is5e = false;
				IsStarfinder = true;
			}
			else if (selectedCharacter is DnD5eCharacter)
			{
				Is5e = true;
				IsStarfinder = false;
			}
		}
	}
}
