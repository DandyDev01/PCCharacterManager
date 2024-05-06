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
				SetViewFlags();

				switch (_selectedCharacterType)
				{
					case CharacterType.starfinder:
						SelectedCreator = StarfinderCharacterCreatorVM;
						break;
					case CharacterType.DnD5e:
						SelectedCreator = DnD5eCharacterCreator;
						break;
				}
			}
		}

		private bool _isStarfinderCharacter;
		public bool IsStarfinderCharacter
		{
			get
			{
				return _isStarfinderCharacter;
			}
			private set
			{
				OnPropertyChanged(ref _isStarfinderCharacter, value);
			}
		}

		private bool _isDnD5eCharacter = true;
		public bool IsDnD5eCharacter
		{
			get
			{
				return _isDnD5eCharacter;
			}
			private set
			{
				OnPropertyChanged(ref _isDnD5eCharacter, value);
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
		public Array CharacterTypes { get; } = Enum.GetValues(typeof(CharacterType));

		public DialogWindowCharacterCreaterViewModel(CharacterStore characterStore, DialogService dialogService)
		{
			DnD5eCharacterCreator = new CharacterCreatorViewModel(dialogService);
			StarfinderCharacterCreatorVM = new StarfinderCharacterCreatorViewModel(dialogService);
			_selectedCreator = DnD5eCharacterCreator;

			_characterStore = characterStore;
		}

		private void SetViewFlags()
		{
			switch (_selectedCharacterType)
			{
				case CharacterType.starfinder:
					IsDnD5eCharacter = false;
					IsStarfinderCharacter = true;
					break;
				case CharacterType.DnD5e:
					IsDnD5eCharacter = true;
					IsStarfinderCharacter = false;
					break;
			}
		}

		public void Create()
		{
			DnD5eCharacter? character = _selectedCharacterType switch
			{
				CharacterType.starfinder => StarfinderCharacterCreatorVM.Create(),
				CharacterType.DnD5e => DnD5eCharacterCreator.Create(),
				_ => throw new Exception("SelectedCharacterType issue"),
			};

			if (character == null) 
				return;

			_characterStore.CreateCharacter(character);
		}
	}
}
