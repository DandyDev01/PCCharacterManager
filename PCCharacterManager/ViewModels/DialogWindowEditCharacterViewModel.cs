﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
		private readonly DnD5eCharacter character;

		public DnD5eCharacter Character
		{
			get { return character; }
		}

		private string language;
		public string Language
		{
			get { return language; }
			set { OnPropertyChanged(ref language, value); }
		}
		private string weapon;
		public string Weapon
		{
			get { return weapon; }
			set { OnPropertyChanged(ref weapon, value); }
		}
		private string armor;
		public string Armor
		{
			get { return armor; }
			set { OnPropertyChanged(ref armor, value); }
		}
		private string tool;
		public string Tool
		{
			get { return tool; }
			set { OnPropertyChanged(ref tool, value); }
		}
		private string otherProf;
		public string OtherProf
		{
			get { return otherProf; }
			set { OnPropertyChanged(ref otherProf, value); }
		}
		public Property movement;
		public Property Movement
		{
			get { return movement; }
			set { OnPropertyChanged(ref movement, value); }
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

		public DialogWindowEditCharacterViewModel(Window _window, DnD5eCharacter _character)
		{
			window = _window;
			character = _character;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);

			language = string.Empty;
			weapon = string.Empty;
			armor = string.Empty;
			tool = string.Empty;
			otherProf = string.Empty;
			movement = new Property();

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
			AddTo(character.Languages);
		}
		public void RemoveLanguage()
		{
			character.Languages.Remove(language);
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

			character.MovementTypes_Speeds.Add(new Property(windowVM.Answer, windowVM1.Answer));
		}
		public void RemoveMovement()
		{
			character.MovementTypes_Speeds.Remove(movement);
		}
		public void AddWeapon()
		{
			AddTo(character.WeaponProficiencies);
		}
		public void RemoveWeapon()
		{
			character.WeaponProficiencies.Remove(weapon);
		}
		public void AddArmor()
		{
			AddTo(character.ArmorProficiencies);
		}
		public void RemoveArmor()
		{
			character.ArmorProficiencies.Remove(armor);
		}
		public void AddTool()
		{
			AddTo(character.ToolProficiences);
		}
		public void RemoveTool()
		{
			character.ToolProficiences.Remove(tool);
		}
		public void AddOtherProf()
		{
			AddTo(character.OtherProficiences);
		}
		public void RemoveOtherProf()
		{
			character.OtherProficiences.Remove(otherProf);
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
