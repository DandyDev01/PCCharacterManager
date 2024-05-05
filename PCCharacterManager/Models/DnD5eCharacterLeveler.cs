﻿using PCCharacterManager.DialogWindows;
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

			if (data == null)
				throw new Exception("The class " + className + " does not exist");

			foreach (var item in data.Features)
			{
				if (item.Level == character.Level.Level)
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
		
		protected override AddClassHelper AddClass(DnD5eCharacter character)
		{
			AddClassHelper helper = new AddClassHelper();
			helper.didAddClass = false;

			var results = MessageBox.Show("Would you like to add another class?", 
				"Add Class", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (results == MessageBoxResult.Yes)
			{
				var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
				string[] classNames = new string[classes.Length];

				for (int i = 0; i < classes.Length; i++)
				{
					classNames[i] = classes[i].Name;
				}

				Window selectClassWindow = new SelectStringValueDialogWindow();
				DialogWindowSelectStingValue vm = 
					new DialogWindowSelectStingValue(selectClassWindow, classNames, 1);
				selectClassWindow.DataContext = vm;
				selectClassWindow.ShowDialog();
				
				string nameOfSelectedClass = vm.SelectedItems.First();

				string currentClassName = character.CharacterClass.Name;
				character.CharacterClass.Name = currentClassName + " " + character.CharacterClass.Level.Level + 
					" / " + nameOfSelectedClass + " " + 1;

				helper.didAddClass = true;
				helper.newClassName = nameOfSelectedClass;
				return helper;
			}
			else if (results == MessageBoxResult.No)
			{
				return helper;
			}
			else if (results == MessageBoxResult.Cancel)
			{
				return helper;
			}

			return helper;
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
				helper.classLevel = character.CharacterClass.Level.Level;
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

			// update Character className
			classNames = character.CharacterClass.Name.Split("/");
			for (int i = 0; i < classNames.Length; i++)
			{
				if (classNames[i].Contains(nameOfSelectedClass))
				{
					classNames[i] = nameOfSelectedClass + " " + (level + 1);
				}
			}

			character.CharacterClass.Name = string.Empty;
			for (int i = 0; i < classNames.Length; i++)
			{
				character.CharacterClass.Name += classNames[i];
				if (i < classNames.Length - 1)
					character.CharacterClass.Name += " / ";
			}

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

			if (level <= 0)
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
