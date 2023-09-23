using PCCharacterManager.DialogWindows;
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

		public Array HitDice { get; } = Enum.GetValues(typeof(HitDie));
		public DnD5eCharacter Character { get; }
		public PropertyListViewModel MovementTypesListVM { get; }
		public StringListViewModel LanguagesVM { get; }
		public StringListViewModel ArmorProfsVM { get; }
		public StringListViewModel WeaponProfsVM { get; }
		public StringListViewModel ToolProfsVM { get; }
		public StringListViewModel OtherProfsVM { get; }
		
		public ICommand OkCommand { get; }

		public DialogWindowEditCharacterViewModel(Window _window, DnD5eCharacter _character)
		{
			window = _window;
			Character = _character;

			OkCommand = new RelayCommand(Ok);

			MovementTypesListVM = new PropertyListViewModel("Movement", Character.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", Character.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", Character.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", Character.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", Character.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", Character.WeaponProficiencies);
			OnPropertyChanged(nameof(MovementTypesListVM));
			OnPropertyChanged(nameof(LanguagesVM));
			OnPropertyChanged(nameof(ArmorProfsVM));
			OnPropertyChanged(nameof(WeaponProfsVM));
			OnPropertyChanged(nameof(ToolProfsVM));
			OnPropertyChanged(nameof(OtherProfsVM));
		}

		public void Ok()
		{
			window.Close();
		}
	}
}
