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
		private DnD5eCharacter? selectedCharacter;
		public DnD5eCharacter? SelectedCharacter
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

		public PropertyListViewModel MovementTypesListVM { get; protected set; }
		public PropertyListViewModel FeaturesListVM { get; protected set; }

		public StringListViewModel LanguagesVM { get; protected set; }
		public StringListViewModel ArmorProfsVM { get; protected set; }
		public StringListViewModel WeaponProfsVM { get; protected set; }
		public StringListViewModel ToolProfsVM { get; protected set; }
		public StringListViewModel OtherProfsVM { get; protected set; }

		public ObservableCollection<Feature> AllFeatures { get; set; }
		private Property? selectedProperty;
		public Property? SelectedProperty
		{
			get
			{
				return selectedProperty;
			}
			set
			{
				OnPropertyChanged(ref selectedProperty, value);
			}
		}

		public CharacterInfoViewModel(CharacterStore _characterStore) 
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			if (selectedCharacter is null)
				return;

			FeaturesListVM = new PropertyListViewModel("Features", null);
			MovementTypesListVM = new PropertyListViewModel("Movement", selectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			OnPropertyChanged(nameof(MovementTypesListVM));
			OnPropertyChanged(nameof(LanguagesVM));
			OnPropertyChanged(nameof(ArmorProfsVM));
			OnPropertyChanged(nameof(WeaponProfsVM));
			OnPropertyChanged(nameof(ToolProfsVM));
			OnPropertyChanged(nameof(OtherProfsVM));

			AllFeatures = new ObservableCollection<Feature>();
			OnPropertyChanged(nameof(AllFeatures));
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			if (selectedCharacter is null)
				return;

			FeaturesListVM = new PropertyListViewModel("Features", null);
			MovementTypesListVM = new PropertyListViewModel("Movement", selectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			OnPropertyChanged(nameof(MovementTypesListVM));
			OnPropertyChanged(nameof(LanguagesVM));
			OnPropertyChanged(nameof(ArmorProfsVM));
			OnPropertyChanged(nameof(WeaponProfsVM));
			OnPropertyChanged(nameof(ToolProfsVM));
			OnPropertyChanged(nameof(OtherProfsVM));

			List<Feature> temp = new();

			foreach (var item in SelectedCharacter.CharacterClass.Features)
			{
				temp.Add(new Feature(item.Name, item.Desc, selectedCharacter.CharacterClass.Name, item.Level.ToString()));
			}

			foreach (var item in SelectedCharacter.Race.Features)
			{
				temp.Add(new Feature(item.Name, item.Desc, selectedCharacter.Race.Name, "-"));
			}

			foreach (var item in selectedCharacter.Race.RaceVariant.Properties)
			{
				temp.Add(new Feature(item.Name, item.Desc, selectedCharacter.Race.RaceVariant.Name, "-"));
			}

			
			AllFeatures = new ObservableCollection<Feature>(temp);
			OnPropertyChanged(nameof(AllFeatures));
		}
	}
}
