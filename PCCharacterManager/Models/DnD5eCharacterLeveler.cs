using PCCharacterManager.DialogWindows;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Models
{
    class DnD5eCharacterLeveler : CharacterLeveler
    {
		protected override MultiClass UpdateMaxHealth(DnD5eCharacter character)
		{
			MultiClass helper = MultiClassHelper(character);
			helper.success = false;

			var message = MessageBox.Show("Would you like to manually enter a new max health",
				"", MessageBoxButton.YesNoCancel);

			if (message == MessageBoxResult.Yes)
			{
				return ManuallyEnterNewMaxHealth(character, helper);
			}
			else if (message == MessageBoxResult.No)
			{
				return AutoRollNewMaxHealth(character, helper);
			}
			else if (message == MessageBoxResult.Cancel)
			{
				return helper;
			}

			return helper;
		}

		protected override void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel)
		{
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);

			// find character class
			DnD5eCharacterClassData? data = classes.Find(x => x.Name.Equals(className));

			// className contains extra data, cut it off and search again.
			data ??= classes.Find(x => x.Name.Equals(className.Substring(0, className.IndexOf(" "))));

			if (data == null)
				throw new Exception("The class " + className + " does not exist");

			foreach (var item in data.Features)
			{
				if (item.Level == classLevel)
				{
					if (item.Name.ToLower().Contains("ability score"))
					{
						var message = MessageBox.Show(item.Desc, "You get an ability score improvement", MessageBoxButton.OK);
						continue;
					}

					character.CharacterClass.Features.Add(item);
				}
			}
		}
		
		protected override AddClassHelper AddClass(DnD5eCharacter character, string classToAddName)
		{
			AddClassHelper helper = new AddClassHelper();

			string currentClassName = character.CharacterClass.Name;

			int numberOfClasses = 1;
			if (currentClassName.Contains("/"))
			{
				numberOfClasses = currentClassName.Split("/").Length;
			}

			if (numberOfClasses == 1)
			{
				character.CharacterClass.Name = currentClassName +
					" / " + classToAddName + " " + 0;
			}
			else
			{
				character.CharacterClass.Name = currentClassName +
					" / " + classToAddName + " " + 1;
			}


			helper.didAddClass = true;
			helper.newClassName = classToAddName;
			return helper;
		}

		protected override bool MeetsPrerequisites(DnD5eCharacter character, CharacterMultiClassData characterMultiClassData)
		{
			string[] prerequisites = characterMultiClassData.Prerequisites.Split('^', '&');
			int[] score = new int[prerequisites.Length];

			// get ability prerequisite name and score.
			for (int i = 0; i < prerequisites.Length; i++)
			{
				prerequisites[i] = prerequisites[i].Trim();
				if (int.TryParse(prerequisites[i].Substring(prerequisites[i].IndexOf(" ")).Trim(), out score[i]) == false)
				{
					throw new Exception("Could not find score.");
				}
				prerequisites[i] = prerequisites[i].Substring(0, prerequisites[i].IndexOf(" ")).Trim();
			}

			// prerequsite contains an OR
			if (characterMultiClassData.Prerequisites.Contains('^'))
			{
				bool meetsOne = false;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score >= score[i])
						meetsOne = true;
				}

				if (meetsOne)
					return true;
			}
			// prerequisite contains an AND
			else if (characterMultiClassData.Prerequisites.Contains('&'))
			{
				bool meetsAll = true;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score < score[i])
						meetsAll = false;
				}

				if (meetsAll)
					return true;
			}
			// there is only a single prerequisite 
			else
			{
				string abilityname = characterMultiClassData.Prerequisites;
				int abilityScore = 0;
				if (int.TryParse(abilityname.Substring(abilityname.IndexOf(" ")).Trim(), out abilityScore) == false)
				{
					throw new Exception("Could not find score.");
				}
				abilityname = abilityname.Substring(0, abilityname.IndexOf(" ")).Trim();
				Ability a = character.Abilities.Where(x => x.Name == abilityname).First();

				if (a.Score >= abilityScore)
					return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Gets information on the characters classes. Specifically, the class selected if there is
		/// more than one class. Otherwise, information about the only class the character has.
		/// </summary>
		/// <param name="character">The character being leveledup.</param>
		/// <returns>Information on the class taking a level.</returns>
		private MultiClass MultiClassHelper(DnD5eCharacter character)
		{
			MultiClass helper = new MultiClass();

			// Character is not multiclassing.
			if (character.CharacterClass.Name.Contains("/") == false)
			{
				helper.hitDie = character.CharacterClass.HitDie.ToString();
				helper.classLevel = character.CharacterClass.Level.Level + 1;
				helper.className = character.CharacterClass.Name;
				return helper;
			}

			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();

			// character is multiclassing. Get information on the class they wish to 
			// level up.
			string nameOfSelectedClass = SelectClassToLevelup(character);
			string hitDie = classes.Where(x => x.Name == nameOfSelectedClass).First().HitDie.ToString();

			string[] classNames = character.CharacterClass.Name.Split("/");
			int level = GetCurrentLevelOfClassBeingLeveledUp(character, nameOfSelectedClass, classNames);

			helper.hitDie = hitDie;
			helper.className = nameOfSelectedClass;
			helper.classLevel = level + 1;

			return helper;
		}

		/// <summary>
		/// Extracts the current level of a class from a string.
		/// </summary>
		/// <param name="character">Character the operation is being performed on.</param>
		/// <param name="nameOfSelectedClass">Name of the class that we want the level from.</param>
		/// <param name="classNames">Names of the classes the character has at least one level in.</param>
		/// <returns>The level found in nameOfSelectedClass</returns>
		/// <exception cref="Exception">When no level is found in nameOfSelectedClass</exception>
		private static int GetCurrentLevelOfClassBeingLeveledUp(DnD5eCharacter character, string nameOfSelectedClass, string[] classNames)
		{
			int level = 0;
			foreach (var item in classNames)
			{
				if (item.Contains(nameOfSelectedClass))
				{
					string temp = item.Trim();
					int.TryParse(temp.Substring(temp.IndexOf(" ")), out level);
				}
			}

			if (level < 0)
				throw new Exception("Invalid Level: " + character.CharacterClass.Name);

			return level;
		}

		/// <summary>
		/// Lets the user select which of their classes they want to take a level in.
		/// </summary>
		/// <param name="character">The character that is being leveled up.</param>
		/// <returns>The name of the class to level up.</returns>
		private static string SelectClassToLevelup(DnD5eCharacter character)
		{
			string[] classNames = character.CharacterClass.Name.Split("/");

			for (int i = 0; i < classNames.Length; i++)
			{
				classNames[i] = classNames[i].Trim();
				classNames[i] = classNames[i].Substring(0, classNames[i].IndexOf(" ")).Trim();
			}

			Window selectClassWindow = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue vm =
				new DialogWindowSelectStingValue(selectClassWindow, classNames, 1);
			selectClassWindow.DataContext = vm;
			selectClassWindow.ShowDialog();

			return vm.SelectedItems.First();
		}

		/// <summary>
		/// Calculate a new max health for the character.
		/// </summary>
		/// <param name="character">Character being leveled up</param>
		/// <param name="helper">Information about the class being leveled up.</param>
		/// <returns>Information about the levelup operation.</returns>
		private static MultiClass AutoRollNewMaxHealth(DnD5eCharacter character, MultiClass helper)
		{
			string classHitDie = helper.hitDie;

			int numRolls = 1;
			int sides = (int.Parse(classHitDie.Substring(classHitDie.IndexOf('D') + 1)));

			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(sides, numRolls);
			int currHealth = character.Health.MaxHealth;

			character.Health.SetMaxHealth(currHealth + numToAddToHealth);

			helper.success = true;
			return helper;
		}

		/// <summary>
		/// User enters a new max health for their character.
		/// </summary>
		/// <param name="character">The character that is being leveledup.</param>
		/// <param name="helper">Information about the class being leveled up.</param>
		/// <returns>Information about the levelup operation.</returns>
		private static MultiClass ManuallyEnterNewMaxHealth(DnD5eCharacter character, MultiClass helper)
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel(window);
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return helper;

			int amount = int.Parse(windowVM.Answer);
			character.Health.SetMaxHealth(amount);

			helper.success = true;
			return helper;
		}
	}
}
