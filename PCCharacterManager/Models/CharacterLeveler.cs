using PCCharacterManager.Commands;
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
	public abstract class CharacterLeveler
	{
		private OpenMessageBoxCommand _askToAddClassCommand = new("Would you like to add another class?",
				"Add Class", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
		private OpenDialogWindowCommand<DialogWindowSelectStingValue> _selectClassToAddCommand;

		public void LevelCharacter(DnD5eCharacter character)
		{
			MessageBoxResult result = AskToAddClass();
			if (result == MessageBoxResult.Yes) 
			{
				string classToAddName = GetClassToAddName(character, character.CharacterClass.Name);

				if (classToAddName == string.Empty)
					return;

				AddClassHelper addClassHelper = AddClass(character, classToAddName);

				if (addClassHelper.didAddClass)
				{
					MultiClass helper = UpdateMaxHealth(character);
					if (helper.success == false)
						return;

					UpdateCharacterClassName(character, helper.className, helper.classLevel);
					UnLockClassFeatures(character, helper.className, helper.classLevel);
					AddNewClassProficiences(character, helper.className);
				}
			}
			else if (result == MessageBoxResult.Cancel)
			{
				return;
			}
			else if (result == MessageBoxResult.No)
			{
				MultiClass helper = UpdateMaxHealth(character);
				if (helper.success == false)
					return;

				character.CharacterClass.Level.LevelUp();

				UpdateCharacterClassName(character, helper.className, helper.classLevel);
				UnLockClassFeatures(character, helper.className, helper.classLevel);
			}

			character.Level.LevelUp();
			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}
		}

		/// <summary>
		/// Add the proficiences of a class to a character.
		/// </summary>
		/// <param name="character">Character to add the proficines too.</param>
		/// <param name="className">Name of the class to get the proficiences from.</param>
		/// <exception cref="Exception"></exception>
		private void AddNewClassProficiences(DnD5eCharacter character, string className)
		{
			var classData = ReadWriteJsonCollection<CharacterMultiClassData>
			.ReadCollection(DnD5eResources.MultiClassDataJson).Where(x => x.Name == className).First();

			if (classData is null)
				throw new Exception("Could not find data for class " + className);

			
			character.ToolProficiences.AddRange(classData.ToolProficiences.Where(x => character.ToolProficiences.Contains(x) == false));
			character.WeaponProficiencies.AddRange(classData.WeaponProficiencies.Where(x => character.WeaponProficiencies.Contains(x) == false));
			character.ArmorProficiencies.AddRange(classData.ArmorProficiencies.Where(x => character.ArmorProficiencies.Contains(x) == false));

			string[] proficientSkills = Ability.GetProficientSkillNames(character.Abilities);
			string[] options = classData.PossibleSkillProficiences.Where(x => proficientSkills.Contains(x) == false).ToArray();
			Window selectSkillWindow = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue vm = new(selectSkillWindow, options, 
				classData.numOfSkillProficiences);
		}

		/// <summary>
		/// Update the character class name to show all the classes the character has a level in.
		/// Will also show the level of each class.
		/// </summary>
		/// <param name="character">Character whose class name to update.</param>
		/// <param name="nameOfClassToUpdate">Name of the class being updated.</param>
		/// <param name="level">Level of the class being updated.</param>
		private void UpdateCharacterClassName(DnD5eCharacter character, string nameOfClassToUpdate, int level)
		{
			string[] classNames = character.CharacterClass.Name.Split("/");
			classNames = character.CharacterClass.Name.Split("/");
			for (int i = 0; i < classNames.Length; i++)
			{
				if (classNames[i].Contains(nameOfClassToUpdate))
				{
					int length = nameOfClassToUpdate.IndexOf(" ") == -1 ? nameOfClassToUpdate.Length : nameOfClassToUpdate.IndexOf(" ");
					classNames[i] = nameOfClassToUpdate.Substring(0, length) + " " + level;
				}
			}

			character.CharacterClass.Name = string.Empty;
			for (int i = 0; i < classNames.Length; i++)
			{
				character.CharacterClass.Name += classNames[i];
				if (i < classNames.Length - 1)
					character.CharacterClass.Name += " / ";
			}

		}

		/// <summary>
		/// Ask the user if they want to add a class.
		/// </summary>
		/// <param name="character"></param>
		/// <returns>Weather or not a class will be added.</returns>
		private MessageBoxResult AskToAddClass()
		{
			_askToAddClassCommand.Execute(this);

			return _askToAddClassCommand.Result;
		}

		/// <summary>
		/// Get the name of the class to add.
		/// </summary>
		/// <returns>Name of the class the user wants to add.</returns>
		private string GetClassToAddName(DnD5eCharacter character, string currentClasses)
		{
			var classes = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray();
			string[] classNames = new string[classes.Length];

			for (int i = 0; i < classes.Length; i++)
			{
				// exclude classes the character already has and classes the character does
				// not meet the prerequisites for.
				if (currentClasses.Contains(classes[i].Name) || MeetsPrerequisites(character, classes[i]) == false)
					continue;

				classNames[i] = classes[i].Name;
			}

			classNames = classNames.Where(x => x is not null).ToArray();

			Window selectClassWindow = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue vm =
				new DialogWindowSelectStingValue(selectClassWindow, classNames, 1);

			_selectClassToAddCommand = new(selectClassWindow, vm);
			_selectClassToAddCommand.Execute(this);

			if (selectClassWindow.DialogResult == false)
				return string.Empty;

			return vm.SelectedItems.First();
		}

		/// <summary>
		/// Determines if the character meets the prerequisites for the class they want to multiclass in.
		/// </summary>
		/// <param name="character">Character that is being checked.</param>
		/// <param name="characterMultiClassData">Multiclass prerequisite data.</param>
		/// <returns></returns>
		protected abstract bool MeetsPrerequisites(DnD5eCharacter character, CharacterMultiClassData characterMultiClassData);

		/// <summary>
		/// Add a class to a character.
		/// </summary>
		/// <param name="character">Character to add the class too.</param>
		/// <param name="classToAddName">Name of the class the character is getting.</param>
		/// <returns></returns>
		protected abstract AddClassHelper AddClass(DnD5eCharacter character, string classToAddName);

		/// <summary>
		/// Update the max health of a character.
		/// </summary>
		/// <param name="character">The character to update the max health of.</param>
		/// <returns></returns>
		protected abstract MultiClass UpdateMaxHealth(DnD5eCharacter character);

		/// <summary>
		/// Unlock the features of a specified class for a specified level.
		/// </summary>
		/// <param name="character">Character that will get the features.</param>
		/// <param name="className">Name of the class whose features are being unlocked.</param>
		/// <param name="classLevel">Level of the features being unlocked.</param>
		protected abstract void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel);
	}

	public struct AddClassHelper
	{
		public bool didAddClass = false;
		public string newClassName = string.Empty;

		public AddClassHelper()
		{
		}
	}

	public struct MultiClass
	{
		public string hitDie = string.Empty;
		public string className = string.Empty;
		public int classLevel = 1;
		public bool success = false;

		public MultiClass()
		{
		}
	}
}
