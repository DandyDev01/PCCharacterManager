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
		private readonly Window window;
		private readonly CharacterStore characterStore;

		private CharacterType selectedCharacterType;
		public CharacterType SelectedCharacterType
		{
			get
			{
				return selectedCharacterType;
			}
			set
			{
				OnPropertyChanged(ref selectedCharacterType, value);
				SetViewFlags();

				switch (selectedCharacterType)
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

		private bool isStarfinderCharacter;
		public bool IsStarfinderCharacter
		{
			get
			{
				return isStarfinderCharacter;
			}
			private set
			{
				OnPropertyChanged(ref isStarfinderCharacter, value);
			}
		}

		private bool isDnD5eCharacter = true;
		public bool IsDnD5eCharacter
		{
			get
			{
				return isDnD5eCharacter;
			}
			private set
			{
				OnPropertyChanged(ref isDnD5eCharacter, value);
			}
		}

		private CharactorCreatorViewModelBase selectedCreator;
		public CharactorCreatorViewModelBase SelectedCreator
		{
			get
			{
				return selectedCreator;
			}
			set
			{
				OnPropertyChanged(ref selectedCreator, value);
			}
		}

		public CharacterCreatoreViewModel DnD5eCharacterCreator { get; }
		public StarfinderCharacterCreatorViewModel StarfinderCharacterCreatorVM { get; }
		public Array CharacterTypes { get; } = Enum.GetValues(typeof(CharacterType));

		#region Commands
		public ICommand CreateCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		#endregion

		public DialogWindowCharacterCreaterViewModel(CharacterStore _characterStore, Window _window)
		{
			DnD5eCharacterCreator = new CharacterCreatoreViewModel();
			StarfinderCharacterCreatorVM = new StarfinderCharacterCreatorViewModel();
			selectedCreator = DnD5eCharacterCreator;

			window = _window;
			characterStore = _characterStore;

			CreateCommand = new RelayCommand(Create);
			CloseCommand = new RelayCommand(Close);
		}

		private void SetViewFlags()
		{
			switch (selectedCharacterType)
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

		private void Create()
		{
			DnD5eCharacter character;
			switch (selectedCharacterType)
			{
				case CharacterType.starfinder:
					character = StarfinderCharacterCreatorVM.Create();
					break;
				case CharacterType.DnD5e:
					character = DnD5eCharacterCreator.Create();
					break;
				default:
					throw new Exception("SelectedCharacterType issue");
			}


			if(character == null) return;

			characterStore.CreateCharacter(character);
			window.DialogResult = true;
			window.Close();
		}

		private void Close()
		{
			window.DialogResult = false;
			window.Close();
		}
	}
}
