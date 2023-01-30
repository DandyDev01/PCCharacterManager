using PCCharacterManager.Commands;
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
	public class CharacterInfoViewModel : ObservableObject
	{
		private DnD5eCharacter selectedCharacter;
		public DnD5eCharacter SelectedCharacter
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

		private string selectedLanguage;
		public string SelectedLanguage
		{
			get
			{
				return "Remove " + selectedLanguage;
			}
			set
			{
				OnPropertyChanged(ref selectedLanguage, value);
			}
		}

		private string selectedWeaponProf;
		public string SelectedWeaponProf
		{
			get
			{
				return "Remove " + selectedWeaponProf;
			}
			set
			{
				OnPropertyChanged(ref selectedWeaponProf, value);
			}
		}

		private string selectedArmorProf;
		public string SelectedArmorProf
		{
			get
			{
				return "Remove " + selectedArmorProf;
			}
			set
			{
				OnPropertyChanged(ref selectedArmorProf, value);
			}
		}

		private string selectedOtherProf;
		public string SelectedOtherProf
		{
			get
			{
				return "Remove " + selectedOtherProf;
			}
			set
			{
				OnPropertyChanged(ref selectedOtherProf, value);
			}
		}

		private string selectedToolProf;
		public string SelectedToolProf
		{
			get
			{
				return "Remove " + selectedToolProf;
			}
			set
			{
				OnPropertyChanged(ref selectedToolProf, value);
			}
		}

		private string  selectedClassFeatureName;
		public string  SelectedClassFeatureName
		{
			get
			{
				return selectedClassFeatureName;
			}
			set
			{
				OnPropertyChanged(ref selectedClassFeatureName, value);
			}
		}

		private string selectedRaceFeatureName;
		public string SelectedRaceFeatureName
		{
			get
			{
				return selectedRaceFeatureName;
			}
			set
			{
				OnPropertyChanged(ref selectedRaceFeatureName, value);
			}
		}

		private Property selectedMovementType;
		public Property SelectedMovementType
		{
			get
			{
				return selectedMovementType;
			}
			set
			{
				selectedMovementType = value;
				OnPropertyChanged(ref selectedMovementType, value);
			}
		}

		private Property selectedRaceFeature;
		public Property SelectedRaceFeature
		{
			get
			{
				return selectedRaceFeature;
			}
			set
			{
				OnPropertyChanged(ref selectedRaceFeature, value);
				SelectedRaceFeatureName = "Remove " + value.Name;
			}
		}

		private DnD5eCharacterClassFeature selectedClassFeature;
		public DnD5eCharacterClassFeature SelectedClassFeature
		{
			get
			{
				return selectedClassFeature;
			}
			set
			{
				OnPropertyChanged(ref selectedClassFeature, value);
				SelectedClassFeatureName = "Remove " + value.Name;
			}
		}

		public ICommand AddLanguageCommand { get; }
		public ICommand RemoveLanguageCommand { get; }
		public ICommand AddMovementTypeCommand { get; }
		public ICommand RemoveMovementTypeCommand { get; }
		public ICommand AddOtherProfCommand { get; }
		public ICommand RemoveOtherProfCommand { get; }
		public ICommand AddWeaponProfCommand { get; }
		public ICommand RemoveWeaponProfCommand { get; }
		public ICommand AddArmorProfCommand { get; }
		public ICommand RemoveArmorProfCommand { get; }
		public ICommand AddToolProfCommand { get; }
		public ICommand RemoveToolProfCommand { get; }
		public ICommand AddRaceFeatureCommand { get; }
		public ICommand RemoveRaceFeatureCommand { get; }
		public ICommand EditRaceFeatureCommand { get; }
		public ICommand AddClassFeatureCommand { get; }
		public ICommand RemoveClassFeatureCommand { get; }
		public ICommand EditClassFeatureCommand { get; }

		public CharacterInfoViewModel(CharacterStore _characterStore) 
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			selectedArmorProf = string.Empty;
			selectedLanguage = string.Empty;
			selectedMovementType = null;
			selectedOtherProf = string.Empty;
			selectedToolProf = string.Empty;
			selectedWeaponProf = string.Empty;

			RemoveLanguageCommand = new RelayCommand(RemoveLanguage);
			RemoveArmorProfCommand = new RelayCommand(RemoveArmorProf);
			RemoveWeaponProfCommand = new RelayCommand(RemoveWeaponProf);
			RemoveToolProfCommand = new RelayCommand(RemoveToolProf);
			RemoveOtherProfCommand = new RelayCommand(RemoveOtherProf);
			RemoveMovementTypeCommand = new RelayCommand(RemoveMovementType);
			RemoveClassFeatureCommand = new RelayCommand(RemoveClassFeature);
			RemoveRaceFeatureCommand = new RelayCommand(RemoveRaceFeature);

			AddLanguageCommand = new RelayCommand(AddLanguage);
			AddMovementTypeCommand = new RelayCommand(AddMovement);
			AddWeaponProfCommand = new RelayCommand(AddWeapon);
			AddArmorProfCommand = new RelayCommand(AddArmor);
			AddToolProfCommand = new RelayCommand(AddTool);
			AddOtherProfCommand = new RelayCommand(AddOtherProf);
			AddClassFeatureCommand = new RelayCommand(AddClassFeature);
			AddRaceFeatureCommand = new RelayCommand(AddRaceFeature);

			EditClassFeatureCommand = new RelayCommand(EditClassFeature);
			EditRaceFeatureCommand = new RelayCommand(EditRaceFeature);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;
		}

		private void RemoveLanguage()
		{
			SelectedCharacter.Languages.Remove(selectedLanguage);
		}

		private void RemoveArmorProf()
		{
			SelectedCharacter.ArmorProficiencies.Remove(selectedArmorProf);
		}

		private void RemoveWeaponProf()
		{
			SelectedCharacter.WeaponProficiencies.Remove(selectedWeaponProf);
		}

		private void RemoveOtherProf()
		{
			SelectedCharacter.OtherProficiences.Remove(selectedOtherProf);
		}

		private void RemoveToolProf()
		{
			SelectedCharacter.ToolProficiences.Remove(selectedToolProf);
		}

		private void RemoveMovementType()
		{
			SelectedCharacter.MovementTypes_Speeds.Remove(selectedMovementType);
		}

		private void RemoveClassFeature()
		{
			SelectedCharacter.CharacterClass.Features.Remove(selectedClassFeature);
		}

		private void RemoveRaceFeature()
		{
			SelectedCharacter.Race.Features.Remove(selectedRaceFeature);
		}

		private void AddLanguage()
		{
			AddTo(selectedCharacter.Languages);
		}

		private void AddMovement()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window, "Movement type i.e. FLY");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel(window1, "Movement Speed i.e. 30ft");
			window1.DataContext = windowVM1;
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			selectedCharacter.MovementTypes_Speeds.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void AddClassFeature()
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

			Window window2 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM2 = new DialogWindowStringInputViewModel(window2, "Feature Level");
			window2.DataContext = windowVM2;
			window2.ShowDialog();

			if (window2.DialogResult == false)
				return;

			int level = 0;
			try
			{
				 level = int.Parse(windowVM2.Answer);
			} catch(Exception e)
			{
				MessageBox.Show(e.Message);
				return;
			}
			DnD5eCharacterClassFeature feature =
				new DnD5eCharacterClassFeature(windowVM.Answer, windowVM1.Answer, level);
			selectedCharacter.CharacterClass.Features.Add(feature);
		}

		private void AddRaceFeature()
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

			selectedCharacter.Race.Features.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void AddWeapon()
		{
			AddTo(selectedCharacter.WeaponProficiencies);
		}

		private void AddArmor()
		{
			AddTo(selectedCharacter.ArmorProficiencies);
		}

		private void AddTool()
		{
			AddTo(selectedCharacter.ToolProficiences);
		}

		private void AddOtherProf()
		{
			AddTo(selectedCharacter.OtherProficiences);
		}

		private void EditClassFeature()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = 
				new DialogWindowStringInputViewModel(window, "Edit " + selectedClassFeature.Name);

			windowVM.Answer = selectedClassFeature.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedClassFeature.Desc = windowVM.Answer;
		}

		private void EditRaceFeature()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = 
				new DialogWindowStringInputViewModel(window, "Edit " + selectedRaceFeature.Name);

			windowVM.Answer = selectedRaceFeature.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			selectedRaceFeature.Desc = windowVM.Answer;
		}

		private void AddTo(in ObservableCollection<string> addTo)
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window);
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			addTo.Add(windowVM.Answer);
		}
	}
}
