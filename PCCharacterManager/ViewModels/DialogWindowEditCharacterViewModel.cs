using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowEditCharacterViewModel : ObservableObject
	{
		private readonly Window window;
		private readonly Character character;

		public Character Character
		{
			get { return character; }
		}

		private string language;
		public string Language
		{
			get { return language; }
			set { OnPropertyChaged(ref language, value); }
		}
		private string weapon;
		public string Weapon
		{
			get { return weapon; }
			set { OnPropertyChaged(ref weapon, value); }
		}
		private string armor;
		public string Armor
		{
			get { return armor; }
			set { OnPropertyChaged(ref armor, value); }
		}
		private string tool;
		public string Tool
		{
			get { return tool; }
			set { OnPropertyChaged(ref tool, value); }
		}
		private string otherProf;
		public string OtherProf
		{
			get { return otherProf; }
			set { OnPropertyChaged(ref otherProf, value); }
		}
		public Property movement;
		public Property Movement
		{
			get { return movement; }
			set { OnPropertyChaged(ref movement, value); }
		}

		public ICommand OkCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		public ICommand AddLanguageCommand { get; private set; }
		public ICommand RemoveLanguageCommand { get; private set; }
		public ICommand AddMovementCommand { get; private set; }
		public ICommand RemoveMovementCommand { get; private set; }
		public ICommand AddWeaponCommand { get; private set; }
		public ICommand RemoveWeaponCommand { get; private set; }
		public ICommand AddArmorCommand { get; private set; }
		public ICommand RemoveArmorCommand { get; private set; }
		public ICommand AddToolCommand { get; private set; }
		public ICommand RemoveToolCommand { get; private set; }
		public ICommand AddOtherProfCommand { get; private set; }
		public ICommand RemoveOtherProfCommand { get; private set; }

		public DialogWindowEditCharacterViewModel(Window _window, Character _character)
		{
			window = _window;
			character = _character;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);

			AddLanguageCommand = new RelayCommand(AddLanguage);
			RemoveLanguageCommand = new RelayCommand(RemoveLanguage);
			AddMovementCommand = new RelayCommand(AddMovement);
			RemoveMovementCommand = new RelayCommand(RemoveMovement);
			AddWeaponCommand = new RelayCommand(AddWeapon);
			RemoveWeaponCommand = new RelayCommand(RemoveWeapon);
			AddArmorCommand = new RelayCommand(AddArmor);
			RemoveArmorCommand = new RelayCommand(RemoveArmor);
			AddToolCommand = new RelayCommand(AddTool);
			RemoveToolCommand = new RelayCommand(RemoveTool);
			AddOtherProfCommand = new RelayCommand(AddOtherProf);
			RemoveOtherProfCommand = new RelayCommand(RemoveOtherProf);
		}

		public void Ok()
		{
			Cancel();
		}

		public void Cancel()
		{
			window.Close();
		}

		public void AddLanguage()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			character.Languages.Add(windowVM.Answer);
		}
		public void RemoveLanguage()
		{
			character.Languages.Remove(language);
		}
		public void AddMovement()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window, "Movement type i.e. FLY");
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM1 = new StringInputDialogWindowViewModel(window1, "Movement Speed i.e. 30ft");
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			character.MovementTypes_Speeds.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}
		public void RemoveMovement()
		{
			character.MovementTypes_Speeds.Remove(movement);
		}
		public void AddWeapon()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			character.WeaponProficiencies.Add(windowVM.Answer);
		}
		public void RemoveWeapon()
		{
			character.WeaponProficiencies.Remove(weapon);
		}
		public void AddArmor()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			character.ArmorProficiencies.Add(windowVM.Answer);
		}
		public void RemoveArmor()
		{
			character.ArmorProficiencies.Remove(armor);
		}
		public void AddTool()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			character.ToolProficiences.Add(windowVM.Answer);
		}
		public void RemoveTool()
		{
			character.ToolProficiences.Remove(tool);
		}
		public void AddOtherProf()
		{
			Window window = new StringInputDialogWindow();
			StringInputDialogWindowViewModel windowVM = new StringInputDialogWindowViewModel(window);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			character.OtherProficiences.Add(windowVM.Answer);
		}
		public void RemoveOtherProf()
		{
			character.OtherProficiences.Remove(otherProf);
		}
	}
}
