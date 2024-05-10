using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Models
{
    public class DnD5eDialogStreamCharacterLeveler : CharacterLeveler
    {
		public DnD5eDialogStreamCharacterLeveler(DialogServiceBase dialogService) : base(dialogService)
		{
		}

		/// <summary>
		/// Open a serise of dialog windows to level up the character.
		/// </summary>
		/// <param name="character">The character to levelup</param>
		public override bool LevelCharacter(DnD5eCharacter character)
		{
			if (character is null)
				throw new NullReferenceException("Character cannot be null");

			MessageBoxResult result = AskToAddClass();
			if (result == MessageBoxResult.Yes)
			{
				string classToAddName = GetClassToAddName(character, character.CharacterClass.Name);

				if (classToAddName == string.Empty)
					return false;

				AddClassHelper addClassHelper = AddClass(character, classToAddName);

				if (addClassHelper.didAddClass)
				{
					MultiClass helper = UpdateMaxHealth(character);
					if (helper.success == false)
						return false;

					character.CharacterClass.UpdateCharacterClassName(helper.className, helper.classLevel);
					UnLockClassFeatures(character, helper.className, helper.classLevel);
					AddNewClassProficiences(character, helper.className);
				}
			}
			else if (result == MessageBoxResult.Cancel)
			{
				return false;
			}
			else if (result == MessageBoxResult.No)
			{
				MultiClass helper = UpdateMaxHealth(character);
				if (helper.success == false)
					return false;

				character.CharacterClass.Level.LevelUp();

				character.CharacterClass.UpdateCharacterClassName(helper.className, helper.classLevel);
				UnLockClassFeatures(character, helper.className, helper.classLevel);
			}

			character.Level.LevelUp();
			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}

			return true;
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

			classData.AddProficiences(character);

			string[] proficientSkills = Ability.GetProficientSkillNames(character.Abilities);
			string[] options = classData.PossibleSkillProficiences.Where(x => proficientSkills.Contains(x) == false).ToArray();
			
			if (options.Any())
			{
				DialogWindowSelectStingValueViewModel vm =
					new DialogWindowSelectStingValueViewModel(options, 1);

				string result = string.Empty;
				_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
				{
					result = r;
				});

				if (result == false.ToString())
					return;
			}

			// TODO: do something with the selected item(s).
		}

		/// <summary>
		/// Ask the user if they want to add a class.
		/// </summary>
		/// <param name="character"></param>
		/// <returns>Weather or not a class will be added.</returns>
		private MessageBoxResult AskToAddClass()
		{
			return _dialogService.ShowMessage("Would you like to add another class?",
				"Add Class", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
				if (currentClasses.Contains(classes[i].Name) || classes[i].MeetsPrerequisites(character) == false)
					continue;

				classNames[i] = classes[i].Name;
			}

			classNames = classNames.Where(x => x is not null).ToArray();

			DialogWindowSelectStingValueViewModel vm =
				new DialogWindowSelectStingValueViewModel(classNames, 1);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return string.Empty;

			return vm.SelectedItems.First();
		}


		/// <summary>
		/// Update the max health of a character.
		/// </summary>
		/// <param name="character">The character to update the max health of.</param>
		/// <returns></returns>
		private MultiClass UpdateMaxHealth(DnD5eCharacter character)
		{
			MultiClass helper = MultiClassHelper(character);
			helper.success = false;

			var message = _dialogService.ShowMessage("Would you like to manually enter a new max health",
				"", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

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

		/// <summary>
		/// Unlock the features of a specified class for a specified level.
		/// </summary>
		/// <param name="character">Character that will get the features.</param>
		/// <param name="className">Name of the class whose features are being unlocked.</param>
		/// <param name="classLevel">Level of the features being unlocked.</param>
		private void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel)
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
						var message = _dialogService.ShowMessage(item.Desc, "You get an ability score improvement", 
							MessageBoxButton.OK, MessageBoxImage.None);
						continue;
					}

					character.CharacterClass.Features.Add(item);
				}
			}
		}

		/// <summary>
		/// Add a class to a character.
		/// </summary>
		/// <param name="character">Character to add the class too.</param>
		/// <param name="classToAddName">Name of the class the character is getting.</param>
		/// <returns></returns>
		private AddClassHelper AddClass(DnD5eCharacter character, string classToAddName)
		{
			AddClassHelper helper = new AddClassHelper();

			string currentClassName = character.CharacterClass.Name;

			int numberOfClasses = character.CharacterClass.Name.Split("/").Length;

			if (numberOfClasses == 1)
			{
				character.CharacterClass.Name = currentClassName + " " + character.Level.Level +
					" / " + classToAddName + " " + 0;
			}
			else
			{
				character.CharacterClass.Name = currentClassName + 
					" / " + classToAddName + " " + 0;
			}


			helper.didAddClass = true;
			helper.newClassName = classToAddName;
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
				helper.hitDie = character.CharacterClass.HitDie;
				helper.classLevel = character.CharacterClass.Level.Level + 1;
				helper.className = character.CharacterClass.Name;
				return helper;
			}

			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();

			// character is multiclassing. Get information on the class they wish to 
			// level up.
			string nameOfSelectedClass = SelectClassToLevelup(character);
			HitDie hitDie = classes.Where(x => x.Name == nameOfSelectedClass).First().HitDie;

			string[] classNames = character.CharacterClass.Name.Split("/");
			int level = GetCurrentLevelOfClassBeingLeveledUp(character, nameOfSelectedClass, classNames);

			helper.hitDie = hitDie;
			helper.className = nameOfSelectedClass;
			helper.classLevel = level + 1;
			helper.classData = classes.Where(x => x.Name == nameOfSelectedClass).First();

			return helper;
		}

		/// <summary>
		/// Lets the user select which of their classes they want to take a level in.
		/// </summary>
		/// <param name="character">The character that is being leveled up.</param>
		/// <returns>The name of the class to level up.</returns>
		private string SelectClassToLevelup(DnD5eCharacter character)
		{
			string[] classNames = character.CharacterClass.Name.Split("/");

			for (int i = 0; i < classNames.Length; i++)
			{
				classNames[i] = classNames[i].Trim();
				if (classNames[i].Contains(" ") == true)
					classNames[i] = classNames[i].Substring(0, classNames[i].IndexOf(" ")).Trim();
			}

			
			DialogWindowSelectStingValueViewModel vm =
				new DialogWindowSelectStingValueViewModel(classNames, 1);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return string.Empty;

			return vm.SelectedItems.First();
		}

		/// <summary>
		/// Calculate a new max health for the character.
		/// </summary>
		/// <param name="character">Character being leveled up</param>
		/// <param name="helper">Information about the class being leveled up.</param>
		/// <returns>Information about the levelup operation.</returns>
		private MultiClass AutoRollNewMaxHealth(DnD5eCharacter character, MultiClass helper)
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(helper.hitDie, 1);
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
		private MultiClass ManuallyEnterNewMaxHealth(DnD5eCharacter character, MultiClass helper)
		{
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel();

			windowVM.Answer = character.Health.MaxHealth.ToString();

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return helper;

			int amount = int.Parse(windowVM.Answer);
			character.Health.SetMaxHealth(amount);

			helper.success = true;
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
		public static int GetCurrentLevelOfClassBeingLeveledUp(DnD5eCharacter character, 
			string nameOfSelectedClass, string[] classNames)
		{
			int level = 0;

			var levels = character.CharacterClass.GetClassNamesAndLevels();

			level = levels.Where(x => x.Key == nameOfSelectedClass).First().Value;

			return level;
		}
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
		public HitDie hitDie = 0;
		public string className = string.Empty;
		public int classLevel = 1;
		public bool success = false;
		public DnD5eCharacterClassData classData = null;

		public MultiClass()
		{
			
		}
	}
}
