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

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterCreatorViewModel : ObservableObject
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

		public StarfinderRaceData[] RaceNamesToDisplay { get; }
		public StarfinderClassData[] ClassNamesToDisplay { get; }
		public StarfinderThemeData[] ThemeNamesToDisplay { get; }

		public StarfinderCharacterCreatorViewModel()
		{
			RaceNamesToDisplay = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			ClassNamesToDisplay = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			ThemeNamesToDisplay = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			name = string.Empty;
			selectedRaceData = RaceNamesToDisplay[0];
			selectedClassData = ClassNamesToDisplay[0];
			selectedThemeData = ThemeNamesToDisplay[0];
		}

		public StarfinderCharacter Create()
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
			string[] skill = selectedClassData.ClassSkills.Where(x => x.Contains("Profession")).ToArray();
			foreach (var item in skill)
			{
				string[] options = StringFormater.CreateGroup(item, '^');
				options[0] = options[0].Substring(options[0].IndexOf('(') + 1);
				options[options.Length - 1] = options[options.Length - 1].Substring(0, options[options.Length - 1].Length - 1);
				foreach (var option in options)
				{
					Window window = new SelectStringValueDialogWindow();
					DialogWindowSelectStingValue windowVM = new DialogWindowSelectStingValue(window, options);
					window.DataContext = windowVM;
					window.ShowDialog();

					string selected = windowVM.SelectedItems.First();
					try
					{
						//StarfinderAbility.FindSkill(character.Abilities, selected).ClassSkill = true;
					}
					catch(Exception e)
					{
						MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
						return null;
					}
				}
			}

			character.KeyAbilityScore = selectedClassData.KeyAbilityScore;

			// race ability score increases
			foreach (var item in selectedRaceData.AbilityScoreIncreases)
			{
				increseAmount = StringFormater.GetInt(item);
				abilityName = StringFormater.RemoveInt(item, 'x');
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
	}
}
