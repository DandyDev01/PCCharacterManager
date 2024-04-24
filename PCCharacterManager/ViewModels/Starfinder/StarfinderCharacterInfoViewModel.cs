﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
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
		public PropertyListViewModel RaceFeatureListVM { get; protected set; }
		public PropertyListViewModel ClassFeatureListVM { get; protected set; }
		public PropertyListViewModel RaceVarientListVM { get; protected set; }

		private StarfinderCharacter selectedCharacter;
		public new StarfinderCharacter SelectedCharacter
		{
			get
			{
				return selectedCharacter;
			}
			set
			{
				OnPropertyChanged(ref selectedCharacter, value);
			}
		}

		public string SelectedThemeFeatureName
		{
			get; set;
		}

		private Property? selectedThemeFeature;
		public Property? SelectedThemeFeature
		{
			get
			{
				return selectedThemeFeature;
			}
			set
			{
				OnPropertyChanged(ref selectedThemeFeature, value);
				SelectedThemeFeatureName = "Remove " + SelectedThemeFeature?.Name;
				OnPropertyChanged(nameof(SelectedThemeFeatureName));
			}
		}

		private StarfinderAugmentation? selectedAugmentation;
		public StarfinderAugmentation? SelectedAugmentation
		{
			get
			{
				return selectedAugmentation;
			}
			set
			{
				OnPropertyChanged(ref selectedAugmentation, value);
				OnPropertyChanged(nameof(RemoveSelectedAugmentationText));
				OnPropertyChanged(nameof(EditSelectedAugmentationText));
			}
		}

		public string RemoveSelectedAugmentationText { get { return "Remove " + selectedAugmentation?.Name; } }
		public string EditSelectedAugmentationText { get { return "Edit " + selectedAugmentation?.Name; } }

		public ICommand AddThemeFeatureCommand { get; }
		public ICommand RemoveThemeFeatureCommand { get; }
		public ICommand EditThemeFeatureCommand { get; }
		public ICommand AddAugmentationCommand { get; }
		public ICommand RemoveAugmentationCommand { get; }
		public ICommand EditAugmentationCommand { get; }

		public PropertyListViewModel ThemeListVM { get; private set; }

		public StarfinderCharacterInfoViewModel(CharacterStore _characterStore) 
			: base(_characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChange;

			if (_characterStore.SelectedCharacter is StarfinderCharacter starfinderCharacter)
				selectedCharacter = starfinderCharacter;
			else
				selectedCharacter = new StarfinderCharacter();

			RaceFeatureListVM = new PropertyListViewModel("Features");
			ClassFeatureListVM = new PropertyListViewModel("Features");
			RaceVarientListVM = new PropertyListViewModel("Features");
			ThemeListVM = new PropertyListViewModel("Features");

			AddThemeFeatureCommand = new RelayCommand(AddThemeFeature);
			RemoveThemeFeatureCommand = new RelayCommand(RemoveThemeFeature);
			EditThemeFeatureCommand = new RelayCommand(EditThemeFeature);
			AddAugmentationCommand = new RelayCommand(AddAugmentation);
			RemoveAugmentationCommand = new RelayCommand(RemoveAugmentation);
			EditAugmentationCommand = new RelayCommand(EditRemoveAugmentation);

			SelectedThemeFeatureName = string.Empty;
		}

		private void OnCharacterChange(DnD5eCharacter newCharacter)
		{
			if (newCharacter is StarfinderCharacter starfinderCharacter)
				selectedCharacter = starfinderCharacter;
			else
			{
				selectedCharacter = new StarfinderCharacter();
				return;
			}

			ThemeListVM = new PropertyListViewModel("Themes", selectedCharacter.Theme.Features);
			ClassFeatureListVM = new DnDClassFeatureListViewModel("Class Features", SelectedCharacter.CharacterClass.Features);
			RaceFeatureListVM = new PropertyListViewModel("Race Features", SelectedCharacter.Race.Features);
			
			OnPropertyChanged(nameof(ClassFeatureListVM));
			OnPropertyChanged(nameof(RaceFeatureListVM));
			OnPropertyChanged(nameof(ClassFeatureListVM));
			OnPropertyChanged(nameof(RaceFeatureListVM));
			OnPropertyChanged(nameof(ThemeListVM));

			MovementTypesListVM.UpdateCollection(selectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(selectedCharacter.Languages);
			ToolProfsVM.UpdateCollection(selectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(selectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(selectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(selectedCharacter.WeaponProficiencies);

		}

		private void EditRemoveAugmentation()
		{
			if (selectedAugmentation is null)
				return;

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel viewModel = 
				new(window, "Update description of " + selectedAugmentation.Name);
			window.DataContext = viewModel;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedAugmentation.Description = viewModel.Answer;
		}

		private void RemoveAugmentation()
		{
			if (selectedCharacter is null ||
				selectedAugmentation is null)
				return;

			selectedCharacter.Augmentations.Remove(selectedAugmentation);
		}

		private void AddAugmentation()
		{
			if (selectedCharacter is null)
				return;

			Window window = new AddAugmentationDialogWindow();
			DialogWindowAddAugmentationViewModel windowVM = new(window);
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedCharacter.Augmentations.Add(windowVM.Augmentation);
		}

		private void AddThemeFeature()
		{
			if (selectedCharacter is null)
				return;

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new(window, "Feature Name");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new(window1, "Feature Description");
			window1.DataContext = windowVM1;
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			selectedCharacter.Theme.Features.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void EditThemeFeature()
		{
			if (selectedThemeFeature is null)
				return;

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new(window, "Edit " + selectedThemeFeature.Name);

			windowVM.Answer = selectedThemeFeature.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedThemeFeature.Desc = windowVM.Answer;
		}

		private void RemoveThemeFeature()
		{
			if (selectedCharacter is null ||
				selectedThemeFeature is null)
				return;

			selectedCharacter.Theme.Features.Remove(selectedThemeFeature);
		}
	}
}
