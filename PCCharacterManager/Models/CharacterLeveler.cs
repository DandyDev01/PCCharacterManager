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
		public void LevelCharacter(DnD5eCharacter character)
		{
			MessageBoxResult result = AskToAddClass();
			if (result == MessageBoxResult.Yes) 
			{
				string classToAddName = GetClassToAddName(character.CharacterClass.Name);
				AddClassHelper addClassHelper = AddClass(character, classToAddName);

				if (addClassHelper.didAddClass)
				{
					MultiClass helper = UpdateMaxHealth(character);
					if (helper.success == false)
						return;

					UpdateCharacterClassName(character, helper.className, helper.classLevel);
					UnLockClassFeatures(character, helper.className, helper.classLevel);
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
			return MessageBox.Show("Would you like to add another class?",
				"Add Class", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
		}

		/// <summary>
		/// Get the name of the class to add.
		/// </summary>
		/// <returns>Name of the class the user wants to add.</returns>
		private string GetClassToAddName(string currentClasses)
		{
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>
				.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
			string[] classNames = new string[classes.Length];

			for (int i = 0; i < classes.Length; i++)
			{
				// exclude classes the character already has.
				if (currentClasses.Contains(classes[i].Name))
					continue;

				classNames[i] = classes[i].Name;
			}

			Window selectClassWindow = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue vm =
				new DialogWindowSelectStingValue(selectClassWindow, classNames, 1);
			selectClassWindow.DataContext = vm;
			selectClassWindow.ShowDialog();

			return vm.SelectedItems.First();
		}

		protected abstract AddClassHelper AddClass(DnD5eCharacter character, string classToAddName);

		protected abstract MultiClass UpdateMaxHealth(DnD5eCharacter character);

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
