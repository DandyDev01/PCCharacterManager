using PCCharacterManager.DialogWindows;
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
		private new StarfinderCharacter selectedCharacter;
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

		private Property selectedThemeFeature;
		public Property SelectedThemeFeature
		{
			get
			{
				return selectedThemeFeature;
			}
			set
			{
				OnPropertyChanged(ref selectedThemeFeature, value);
				SelectedThemeFeatureName = "Remove " + SelectedThemeFeature.Name;
				OnPropertyChaged("SelectedThemeFeatureName");
			}
		}

		public ICommand AddThemeFeatureCommand { get; }
		public ICommand RemoveThemeFeatureCommand { get; }
		public ICommand EditThemeFeatureCommand { get; }

		public StarfinderCharacterInfoViewModel(CharacterStore _characterStore) : base(_characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChange;
			AddThemeFeatureCommand = new RelayCommand(AddThemeFeature);
			RemoveThemeFeatureCommand = new RelayCommand(RemoveThemeFeature);
			EditThemeFeatureCommand = new RelayCommand(EditThemeFeature);
		}

		private void OnCharacterChange(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter as StarfinderCharacter;
		}

		private void AddThemeFeature()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window, "Feature Name");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel(window1, "Feature Description");
			window1.DataContext = windowVM1;
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			selectedCharacter.Theme.Features.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void EditThemeFeature()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window, "Edit " + selectedThemeFeature.Name);

			windowVM.Answer = selectedThemeFeature.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedThemeFeature.Desc = windowVM.Answer;
		}

		private void RemoveThemeFeature()
		{
			selectedCharacter.Theme.Features.Remove(selectedThemeFeature);
		}
	}
}
