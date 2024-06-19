using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowCharacterCreaterViewModel : ObservableObject
	{
		private readonly CharacterStore _characterStore;
		private readonly CharacterTypeHelper _characterTypeHelper;

		private CharacterType _selectedCharacterType;
		public CharacterType SelectedCharacterType
		{
			get
			{
				return _selectedCharacterType;
			}
			set
			{
				OnPropertyChanged(ref _selectedCharacterType, value);
				_characterTypeHelper.SetCharacterTypeFlags(_selectedCharacterType);
				SetSelectedCreator();
			}
		}

		private CharactorCreatorViewModelBase _selectedCreator;
		public CharactorCreatorViewModelBase SelectedCreator
		{
			get
			{
				return _selectedCreator;
			}
			set
			{
				OnPropertyChanged(ref _selectedCreator, value);
			}
		}

		public CharacterCreatorViewModel DnD5eCharacterCreator { get; }
		public StarfinderCharacterCreatorViewModel StarfinderCharacterCreatorVM { get; }
		public DarkSoulsCharacterCreatorViewModel DarkSoulsCharacterCreatorVM { get; }
		public Array CharacterTypes { get; } = Enum.GetValues(typeof(CharacterType));
		public CharacterTypeHelper CharacterTypeHelper => _characterTypeHelper;

		public DialogWindowCharacterCreaterViewModel(CharacterStore characterStore, DialogServiceBase dialogService)
		{
			_characterTypeHelper = new CharacterTypeHelper();
			DnD5eCharacterCreator = new CharacterCreatorViewModel(dialogService);
			StarfinderCharacterCreatorVM = new StarfinderCharacterCreatorViewModel(dialogService);
			DarkSoulsCharacterCreatorVM = new DarkSoulsCharacterCreatorViewModel(dialogService);
			_selectedCreator = DnD5eCharacterCreator;

			_characterStore = characterStore;
		}

		private void SetSelectedCreator()
		{
			switch (_selectedCharacterType)
			{
				case CharacterType.starfinder:
					SelectedCreator = StarfinderCharacterCreatorVM;
					break;
				case CharacterType.DnD5e:
					SelectedCreator = DnD5eCharacterCreator;
					break;
				case CharacterType.dark_souls:
					SelectedCreator = DarkSoulsCharacterCreatorVM;
					break;
			}
		}

		public void Create()
		{
			DnD5eCharacter? character = SelectedCreator.Create();

			if (character == null) 
				return;

			character.CharacterType = _selectedCharacterType;

			_characterStore.CreateCharacter(character);
		}
	}
}
