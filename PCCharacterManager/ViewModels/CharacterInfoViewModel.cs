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
	public class CharacterInfoViewModel : TabItemViewModel
	{
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged(ref name, value);
			}
		}

		private string armorClass;
		public string ArmorClass
		{
			get
			{
				return armorClass;
			}
			set
			{
				armorClass = value;
				OnPropertyChanged(ref armorClass, value);
			}
		}

		private string age;
		public string Age
		{
			get
			{
				return age;
			}
			set
			{
				age = value;
				OnPropertyChanged(ref age, value);
			}
		}

		private string currentHitPoints;
		public string CurrentHitPoints
		{
			get
			{
				return currentHitPoints;
			}
			set
			{
				currentHitPoints = value;
				OnPropertyChanged(ref currentHitPoints, value);
			}
		}

		private string temporaryHitPoints;
		public string TemporaryHitPoints
		{
			get
			{
				return temporaryHitPoints;
			}
			set
			{
				temporaryHitPoints = value;
				OnPropertyChanged(ref temporaryHitPoints, value);
			}
		}

		private string experiencePoints;
		public string ExperiencePoints
		{
			get
			{
				return experiencePoints;
			}
			set
			{
				experiencePoints = value;
				OnPropertyChanged(ref experiencePoints, value);
			}
		}

		private string initiative;
		public string Initiative
		{
			get
			{
				return initiative;
			}
			set
			{
				initiative = value;
				OnPropertyChanged(ref initiative, value);
			}
		}

		private string passivePerception;
		public string PassivePerception
		{
			get
			{
				return passivePerception;
			}
			set
			{
				passivePerception = value;
				OnPropertyChanged(ref passivePerception, value);
			}
		}

		private string background;
		public string Background
		{
			get
			{
				return background;
			}
			set
			{
				background = value;
				OnPropertyChanged(ref background, value);
			}
		}

		private string race;
		public string Race
		{
			get
			{
				return race;
			}
			set
			{
				race = value;
				OnPropertyChanged(ref race, value);
			}
		}

		private string raceVarient;
		public string RaceVarient
		{
			get
			{
				return raceVarient;
			}
			set
			{
				raceVarient = value;
				OnPropertyChanged(ref raceVarient, value);
			}
		}

		public ObservableCollection<Property> ClassFeatures { get; }
		public ObservableCollection<Property> RaceFeatures { get; }
		public ObservableCollection<Property> RaceVarientFeatures { get; }

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
				return "Remove" + selectedWeaponProf;
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
				return "Remove" + selectedArmorProf;
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
				return "Remove" + selectedOtherProf;
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
				return "Remove" + selectedToolProf;
			}
			set
			{
				OnPropertyChanged(ref selectedToolProf, value);
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

		public CharacterInfoViewModel(CharacterStore _characterStore, ICharacterDataService _dataService) 
			: base(_characterStore, _dataService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			name = string.Empty;
			armorClass = string.Empty;
			age = string.Empty;
			currentHitPoints = string.Empty;
			temporaryHitPoints = string.Empty;
			experiencePoints = string.Empty;
			initiative = string.Empty;
			passivePerception = string.Empty;
			background = string.Empty;
			race = string.Empty;
			raceVarient = string.Empty;

			ClassFeatures = new ObservableCollection<Property>();
			RaceFeatures = new ObservableCollection<Property>();
			RaceVarientFeatures = new ObservableCollection<Property>();

			RemoveLanguageCommand = new RelayCommand(RemoveLanguage);
			RemoveArmorProfCommand = new RelayCommand(RemoveArmorProf);
			RemoveWeaponProfCommand = new RelayCommand(RemoveWeaponProf);
			RemoveToolProfCommand = new RelayCommand(RemoveToolProf);
			RemoveOtherProfCommand = new RelayCommand(RemoveOtherProf);
			RemoveMovementTypeCommand = new RelayCommand(RemoveMovementType);

			AddLanguageCommand = new RelayCommand(AddLanguage);
			AddMovementTypeCommand = new RelayCommand(AddMovement);
			AddWeaponProfCommand = new RelayCommand(AddWeapon);
			AddArmorProfCommand = new RelayCommand(AddArmor);
			AddToolProfCommand = new RelayCommand(AddTool);
			AddOtherProfCommand = new RelayCommand(AddOtherProf);
		}

		private void RemoveLanguage()
		{
			base.selectedCharacter.Languages.Remove(selectedLanguage);
		}

		private void RemoveArmorProf()
		{
			base.selectedCharacter.ArmorProficiencies.Remove(selectedArmorProf);
		}

		private void RemoveWeaponProf()
		{
			base.selectedCharacter.WeaponProficiencies.Remove(selectedWeaponProf);
		}

		private void RemoveOtherProf()
		{
			base.selectedCharacter.OtherProficiences.Remove(selectedOtherProf);
		}

		private void RemoveToolProf()
		{
			base.selectedCharacter.ToolProficiences.Remove(selectedToolProf);
		}

		private void RemoveMovementType()
		{
			base.selectedCharacter.MovementTypes_Speeds.Remove(selectedMovementType);
		}

		public void AddLanguage()
		{
			AddTo(selectedCharacter.Languages);
		}

		public void AddMovement()
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

		public void AddWeapon()
		{
			AddTo(selectedCharacter.WeaponProficiencies);
		}

		public void AddArmor()
		{
			AddTo(selectedCharacter.ArmorProficiencies);
		}

		public void AddTool()
		{
			AddTo(selectedCharacter.ToolProficiences);
		}

		public void AddOtherProf()
		{
			AddTo(selectedCharacter.OtherProficiences);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected override void OnCharacterChanged(Character newCharacter)
		{
			name = newCharacter.Name;
			armorClass = newCharacter.ArmorClass;
			age = newCharacter.Race.Age.ToString();
			currentHitPoints = newCharacter.Health.CurrHealth.ToString();
			temporaryHitPoints = newCharacter.Health.TempHitPoints.ToString();
			experiencePoints = newCharacter.Level.ExperiencePoints.ToString();
			passivePerception = newCharacter.PassivePerception.ToString();
			background = newCharacter.Background;
			race = newCharacter.Race.Name;
			RaceVarient = newCharacter.Race.RaceVariant.Name;

			foreach (var item in newCharacter.CharacterClass.Features)
			{
				ClassFeatures.Add(item);
			}

			foreach (var item in newCharacter.Race.Features)
			{
				RaceFeatures.Add(item);
			}

			foreach (var item in newCharacter.Race.RaceVariant.Properties)
			{
				RaceVarientFeatures.Add(item);
			}

			SelectedCharacter = newCharacter;
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
