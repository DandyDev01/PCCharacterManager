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

		public PropertyListViewModel RaceFeatureListVM { get; protected set; }
		public PropertyListViewModel ClassFeatureListVM { get; protected set; }
		public PropertyListViewModel MovementTypesListVM { get; protected set; }
		public PropertyListViewModel RaceVarientListVM { get; protected set; }

		public StringListViewModel LanguagesVM { get; protected set; }
		public StringListViewModel ArmorProfsVM { get; protected set; }
		public StringListViewModel WeaponProfsVM { get; protected set; }
		public StringListViewModel ToolProfsVM { get; protected set; }
		public StringListViewModel OtherProfsVM { get; protected set; }

		public CharacterInfoViewModel(CharacterStore _characterStore) 
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;
			ClassFeatureListVM = new DnDClassFeatureListViewModel("Class Features", SelectedCharacter.CharacterClass.Features);
			RaceFeatureListVM = new PropertyListViewModel("Race Features", SelectedCharacter.Race.Features);
			RaceVarientListVM = new PropertyListViewModel("Race Variant Features", selectedCharacter.Race.RaceVariant.Properties);
			MovementTypesListVM = new PropertyListViewModel("Movement", SelectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			OnPropertyChanged("ClassFeatureListVM");
			OnPropertyChanged("RaceFeatureListVM");
			OnPropertyChanged("MovementTypesListVM");
			OnPropertyChanged("LanguagesVM");
			OnPropertyChanged("ArmorProfsVM");
			OnPropertyChanged("WeaponProfsVM");
			OnPropertyChanged("ToolProfsVM");
			OnPropertyChanged("OtherProfsVM");
			OnPropertyChanged("RaceVarientListVM");
		}
	}
}
