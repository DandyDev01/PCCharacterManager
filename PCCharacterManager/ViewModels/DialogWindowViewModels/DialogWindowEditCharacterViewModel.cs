﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
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
		private readonly DialogServiceBase _dialogService;

		public Array HitDice { get; } = Enum.GetValues(typeof(HitDie));
		public DnD5eCharacter Character { get; }
		public PropertyListViewModel MovementTypesListVM { get; }
		public StringListViewModel ArmorProfsVM { get; }
		public StringListViewModel WeaponProfsVM { get; }
		public StringListViewModel OtherProfsVM { get; }
		
		public DialogWindowEditCharacterViewModel(DnD5eCharacter character, DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
			Character = character;

			MovementTypesListVM = new PropertyListViewModel("Movement", Character.MovementTypes_Speeds, _dialogService);
			ArmorProfsVM = new StringListViewModel("Armor Profs", Character.ArmorProficiencies, _dialogService);
			OtherProfsVM = new StringListViewModel("Other Profs", Character.OtherProficiences, _dialogService);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", Character.WeaponProficiencies, _dialogService);
		}
	}
}
