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
	public class StarfinderCharacterInfoViewModel : ObservableObject
	{
		private StarfinderCharacter starfinderCharacter;
		public StarfinderCharacter SelectedCharacter
		{
			get
			{
				return starfinderCharacter;
			}
			set
			{
				OnPropertyChanged(ref starfinderCharacter, value);
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
		public ICommand AddWeaponProfCommand { get; }
		public ICommand RemoveWeaponProfCommand { get; }
		public ICommand AddArmorProfCommand { get; }
		public ICommand RemoveArmorProfCommand { get; }

		public StarfinderCharacterInfoViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChange;

			RemoveLanguageCommand = new RelayCommand(RemoveLanguage);
			RemoveArmorProfCommand = new RelayCommand(RemoveArmorProf);
			RemoveWeaponProfCommand = new RelayCommand(RemoveWeaponProf);
			RemoveMovementTypeCommand = new RelayCommand(RemoveMovementType);

			AddLanguageCommand = new RelayCommand(AddLanguage);
			AddMovementTypeCommand = new RelayCommand(AddMovement);
			AddWeaponProfCommand = new RelayCommand(AddWeapon);
			AddArmorProfCommand = new RelayCommand(AddArmor);
		}

		private void OnCharacterChange(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter as StarfinderCharacter;
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

		private void RemoveMovementType()
		{
			SelectedCharacter.MovementTypes_Speeds.Remove(selectedMovementType);
		}

		private void AddLanguage()
		{
			AddTo(starfinderCharacter.Languages);
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

			starfinderCharacter.MovementTypes_Speeds.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}

		private void AddWeapon()
		{
			AddTo(starfinderCharacter.WeaponProficiencies);
		}

		private void AddArmor()
		{
			AddTo(starfinderCharacter.ArmorProficiencies);
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
