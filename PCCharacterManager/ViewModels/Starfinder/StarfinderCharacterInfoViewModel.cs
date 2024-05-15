using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
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
	public class StarfinderCharacterInfoViewModel : CharacterInfoViewModel
	{
		private readonly DialogServiceBase _dialogService;
		private readonly RecoveryBase _recovery;

		public PropertyListViewModel RaceFeatureListVM { get; protected set; }
		public PropertyListViewModel ClassFeatureListVM { get; protected set; }
		public PropertyListViewModel RaceVarientListVM { get; protected set; }

		private StarfinderCharacter _selectedCharacter;
		public new StarfinderCharacter SelectedCharacter
		{
			get
			{
				return _selectedCharacter;
			}
			set
			{
				OnPropertyChanged(ref _selectedCharacter, value);
			}
		}

		public string SelectedThemeFeatureName
		{
			get; set;
		}

		private Property? _selectedThemeFeature;
		public Property? SelectedThemeFeature
		{
			get
			{
				return _selectedThemeFeature;
			}
			set
			{
				OnPropertyChanged(ref _selectedThemeFeature, value);
				SelectedThemeFeatureName = "Remove " + SelectedThemeFeature?.Name;
				OnPropertyChanged(nameof(SelectedThemeFeatureName));
			}
		}

		private StarfinderAugmentation? _selectedAugmentation;
		public StarfinderAugmentation? SelectedAugmentation
		{
			get
			{
				return _selectedAugmentation;
			}
			set
			{
				OnPropertyChanged(ref _selectedAugmentation, value);
				OnPropertyChanged(nameof(RemoveSelectedAugmentationText));
				OnPropertyChanged(nameof(EditSelectedAugmentationText));
			}
		}

		public string RemoveSelectedAugmentationText { get { return "Remove " + _selectedAugmentation?.Name; } }
		public string EditSelectedAugmentationText { get { return "Edit " + _selectedAugmentation?.Name; } }

		public ICommand AddThemeFeatureCommand { get; }
		public ICommand RemoveThemeFeatureCommand { get; }
		public ICommand EditThemeFeatureCommand { get; }
		public ICommand AddAugmentationCommand { get; }
		public ICommand RemoveAugmentationCommand { get; }
		public ICommand EditAugmentationCommand { get; }

		public PropertyListViewModel ThemeListVM { get; private set; }

		public StarfinderCharacterInfoViewModel(CharacterStore characterStore, DialogServiceBase dialogService, 
			RecoveryBase recovery) 
			: base(characterStore, dialogService, recovery)
		{
			_recovery = recovery;
			characterStore.SelectedCharacterChange += OnCharacterChange;

			if (characterStore.SelectedCharacter is StarfinderCharacter starfinderCharacter)
				_selectedCharacter = starfinderCharacter;
			else
				_selectedCharacter = new StarfinderCharacter();

			RaceFeatureListVM = new PropertyListViewModel("Features", dialogService);
			ClassFeatureListVM = new PropertyListViewModel("Features", dialogService);
			RaceVarientListVM = new PropertyListViewModel("Features", dialogService);
			ThemeListVM = new PropertyListViewModel("Features", dialogService);

			AddThemeFeatureCommand = new RelayCommand(AddThemeFeature);
			RemoveThemeFeatureCommand = new RelayCommand(RemoveThemeFeature);
			EditThemeFeatureCommand = new RelayCommand(EditThemeFeature);
			AddAugmentationCommand = new RelayCommand(AddAugmentation);
			RemoveAugmentationCommand = new RelayCommand(RemoveAugmentation);
			EditAugmentationCommand = new RelayCommand(EditRemoveAugmentation);

			SelectedThemeFeatureName = string.Empty;

			_dialogService = dialogService;
		}

		private void OnCharacterChange(DnD5eCharacter newCharacter)
		{
			if (newCharacter is StarfinderCharacter starfinderCharacter)
				_selectedCharacter = starfinderCharacter;
			else
			{
				_selectedCharacter = new StarfinderCharacter();
				return;
			}

			ThemeListVM = new PropertyListViewModel("Themes", _selectedCharacter.Theme.Features, _dialogService);
			ClassFeatureListVM = new DnDClassFeatureListViewModel("Class Features", 
				SelectedCharacter.CharacterClass.Features, _dialogService);
			RaceFeatureListVM = new PropertyListViewModel("Race Features", SelectedCharacter.Race.Features, 
				_dialogService);
			
			OnPropertyChanged(nameof(ClassFeatureListVM));
			OnPropertyChanged(nameof(RaceFeatureListVM));
			OnPropertyChanged(nameof(ClassFeatureListVM));
			OnPropertyChanged(nameof(RaceFeatureListVM));
			OnPropertyChanged(nameof(ThemeListVM));

			MovementTypesListVM.UpdateCollection(_selectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(_selectedCharacter.Languages);
			ToolProfsVM.UpdateCollection(_selectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(_selectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(_selectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(_selectedCharacter.WeaponProficiencies);

		}

		private void EditRemoveAugmentation()
		{
			if (_selectedAugmentation is null)
				return;

			DialogWindowStringInputViewModel viewModel = new("Update description of " + _selectedAugmentation.Name);
			
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(viewModel, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedAugmentation.Description = viewModel.Answer;
		}

		private void RemoveAugmentation()
		{
			if (_selectedCharacter is null ||
				_selectedAugmentation is null)
				return;

			_selectedCharacter.Augmentations.Remove(_selectedAugmentation);
		}

		private void AddAugmentation()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowAddAugmentationViewModel windowVM = new();
			string result = string.Empty;
			_dialogService.ShowDialog<AddAugmentationDialogWindow, DialogWindowAddAugmentationViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedCharacter.Augmentations.Add(windowVM.Augmentation);
		}

		private void AddThemeFeature()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowStringInputViewModel windowVM = new("Feature Name");
			
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			DialogWindowStringInputViewModel windowVM1 = new("Feature Description");
			result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM1, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedCharacter.Theme.Features.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void EditThemeFeature()
		{
			if (_selectedThemeFeature is null)
				return;

			DialogWindowStringInputViewModel windowVM = new("Edit " + _selectedThemeFeature.Name);

			windowVM.Answer = _selectedThemeFeature.Desc;

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedThemeFeature.Desc = windowVM.Answer;
		}

		private void RemoveThemeFeature()
		{
			if (_selectedCharacter is null ||
				_selectedThemeFeature is null)
				return;

			_selectedCharacter.Theme.Features.Remove(_selectedThemeFeature);
		}
	}
}
