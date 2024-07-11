using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

		private bool _hasData;
		public bool HasData
		{
			get
			{
				return _hasData;
			}
			set
			{
				OnPropertyChanged(ref _hasData, value);
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

			HasData = true;
		}

		private void SetSelectedCreator()
		{
			switch (_selectedCharacterType)
			{
				case CharacterType.starfinder:
					if (Directory.Exists(StarfinderResources.CharacterDataDir) == false)
					{
						HasData = false;
						return;
					}
					SelectedCreator = StarfinderCharacterCreatorVM;
					HasData = true;
					break;
				case CharacterType.DnD5e:
					if (Directory.Exists(DnD5eResources.CharacterDataDir) == false)
					{
						HasData = false;
						return;
					}
					SelectedCreator = DnD5eCharacterCreator;
					HasData = true;
					break;
				case CharacterType.dark_souls:
					if (Directory.Exists(DarkSoulsResources.CharacterDataDir) == false)
					{
						HasData = false;
						return;
					}
					SelectedCreator = DarkSoulsCharacterCreatorVM;
					HasData = true;
					break;
			}
		}

		public void Create()
		{
			CharacterBase? character = SelectedCreator.Create();

			if (character == null) 
				return;

			character.CharacterType = _selectedCharacterType;

			_characterStore.CreateCharacter(character);
		}
	}
}
