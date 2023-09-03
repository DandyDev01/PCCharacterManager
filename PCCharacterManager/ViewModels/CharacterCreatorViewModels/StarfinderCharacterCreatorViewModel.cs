using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.DialogWindows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System.ComponentModel;
using System.Collections;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterCreatorViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
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
				OnPropertyChanged(ref name, value);
				BasicStringFieldValidation(nameof(Name), value);
			}
		}

		private StarfinderRaceData selectedRaceData;
		public StarfinderRaceData SelectedRaceData
		{
			get
			{
				return selectedRaceData;
			}
			set
			{
				OnPropertyChanged(ref selectedRaceData, value);
			}
		}

		private StarfinderClassData selectedClassData;
		public StarfinderClassData SelectedClassData
		{
			get
			{
				return selectedClassData;
			}
			set
			{
				OnPropertyChanged(ref selectedClassData, value);
			}
		}

		private StarfinderThemeData selectedThemeData;
		public StarfinderThemeData SelectedThemeData
		{
			get
			{
				return selectedThemeData;
			}
			set
			{
				OnPropertyChanged(ref selectedThemeData, value);
			}
		}

		private bool isValid;
		public bool IsValid
		{
			get
			{
				return isValid;
			}
			set
			{
				OnPropertyChanged(ref isValid, value);
			}
		}

		public StarfinderRaceData[] RaceNamesToDisplay { get; }
		public StarfinderClassData[] ClassNamesToDisplay { get; }
		public StarfinderThemeData[] ThemeNamesToDisplay { get; }

		public int[] AbilityScores { get; } = new int[6];

		public ICommand RollAbilityScoresCommand { get; private set; }

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public StarfinderCharacterCreatorViewModel()
		{
			propertyNameToError = new Dictionary<string, List<string>>();

			RaceNamesToDisplay = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			ClassNamesToDisplay = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			ThemeNamesToDisplay = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			name = string.Empty;
			selectedRaceData = RaceNamesToDisplay[0];
			selectedClassData = ClassNamesToDisplay[0];
			selectedThemeData = ThemeNamesToDisplay[0];

			RollAbilityScoresCommand = new RelayCommand(AbilityRoll);

			BasicStringFieldValidation(nameof(Name), Name);
		}

		public override StarfinderCharacter? Create()
		{
			int increseAmount = 0;
			string abilityName = string.Empty;

			bool validName = string.IsNullOrWhiteSpace(name) ? false : !string.IsNullOrEmpty(name);

			if (!validName)
			{
				MessageBox.Show("Name cannot be empty or only whitespace", "Invalid Name", 
					MessageBoxButton.OK, MessageBoxImage.Error);

				return null;
			}

			StarfinderCharacter character = new StarfinderCharacter(SelectedClassData, SelectedRaceData, selectedThemeData);

			character.Name = name;
			int hitPoints = selectedRaceData.HitPoitns + selectedClassData.HitPoints;
			int staminaPoints = selectedClassData.StaminaPoints;

			character.StaminaPoints.Desc = staminaPoints.ToString();
			character.Health.SetMaxHealth(hitPoints);

			// set ability scores
			for (int i = 0; i < AbilityScores.Length; i++)
			{
				character.Abilities[i].Score = AbilityScores[i];
			}

			// set class skills
			foreach (var classSkill in selectedClassData.ClassSkills)
			{
				if (classSkill.Contains("Profession")) continue;

				try
				{
					StarfinderAbility.FindSkill(character.Abilities, classSkill).ClassSkill = true;
				} 
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
					return null;
				}
			}

			// class skill profrssion
			string[] skills = selectedClassData.ClassSkills.Where(x => x.Contains("Profession")).ToArray();
			foreach (var item in skills)
			{
				string[] options = StringFormater.CreateGroup(item, '^');
				options[0] = options[0].Substring(options[0].IndexOf('(') + 1);
				options[options.Length - 1] = options[options.Length - 1].Substring(0, options[options.Length - 1].Length - 1);
				
				Window window = new SelectStringValueDialogWindow();
				DialogWindowSelectStingValue windowVM = new DialogWindowSelectStingValue(window, options);
				window.DataContext = windowVM;
				window.ShowDialog();

				string selected = windowVM.SelectedItems.First();

				character.CharacterClass.Features.Add(new DnD5eCharacterClassFeature("Class skill profession", selected, 1));
			}

			character.KeyAbilityScore = selectedClassData.KeyAbilityScore;

			// race ability score increases
			foreach (var item in selectedRaceData.AbilityScoreIncreases)
			{
				increseAmount = StringFormater.FindQuantity(item);
				abilityName = StringFormater.RemoveQuantity(item);
				try
				{
					Ability.FindAbility(character.Abilities, abilityName).Score += increseAmount;
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
					return null;
				}
			}

			// race features
			foreach (var item in selectedRaceData.Features)
			{
				character.Race.Features.Add(item);
			}


			// theme ability score improvements
			increseAmount = StringFormater.FindQuantity(selectedThemeData.AbilityScoreImprovement);
			abilityName = StringFormater.RemoveQuantity(selectedThemeData.AbilityScoreImprovement);

			try
			{
				Ability.FindAbility(character.Abilities, abilityName).Score += increseAmount;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
				return null;
			}

			return character;
		}

		private void AbilityRoll()
		{
			RollDie rollDie = new RollDie();

			for (int i = 0; i < 6; i++)
			{
				AbilityScores[i] = rollDie.AbilityScoreRoll();
				OnPropertyChanged("AbilityScores");
			}
		}

		public IEnumerable GetErrors(string? propertyName)
		{
			return propertyNameToError.GetValueOrDefault(propertyName, new List<string>());
		}

		private void BasicStringFieldValidation(string propertyName, string propertyValue)
		{
			propertyNameToError.Remove(propertyName);

			List<string> errors = new List<string>();
			propertyNameToError.Add(propertyName, errors);
			if (string.IsNullOrEmpty(propertyValue) || string.IsNullOrWhiteSpace(propertyValue))
			{
				propertyNameToError[propertyName].Add("Cannot be empty or white space");
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			}

			if (char.IsWhiteSpace(propertyValue.FirstOrDefault()))
			{
				propertyNameToError[propertyName].Add("Cannot start with white space");
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			}

			if (propertyNameToError[propertyName].Any() == false)
			{
				propertyNameToError.Remove(propertyName);
			}

			IsValid = !HasErrors;
		}
	}
}
