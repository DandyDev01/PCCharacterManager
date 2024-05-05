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
			MultiClass helper = MultiClasHelper(character);
			helper.success = false;

			var message = MessageBox.Show("Would you like to manually enter a new max health",
				"", MessageBoxButton.YesNoCancel);

			if (message == MessageBoxResult.Yes)
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
			else if (message == MessageBoxResult.No)
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
			else if (message == MessageBoxResult.Cancel)
			{
				return helper;
			}

			return helper;
		}

		private MultiClass MultiClasHelper(DnD5eCharacter character)
		{
			MultiClass helper = new MultiClass();
			if (character.CharacterClass.Name.Contains("/") == false)
				return helper;


			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
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

			string nameOfSelectedClass = vm.SelectedItems.First();
			string hitDie = classes.Where(x => x.Name == nameOfSelectedClass).First().HitDie.ToString();
			
			string className = character.CharacterClass.Name;
			className = className.Substring(className.IndexOf(nameOfSelectedClass), nameOfSelectedClass.Length);
			int level;
			int.TryParse(className.Substring(className.IndexOf(" ")), out level);

			if (level <= 0)
				throw new Exception("Invalid Level: " + character.CharacterClass.Name);

			helper.hitDie = hitDie;
			helper.className = nameOfSelectedClass;
			helper.classLevel = level + 1;

			classNames = character.CharacterClass.Name.Split("/");
			for (int i = 0; i < classNames.Length; i++)
			{
				if (classNames[i].Contains(nameOfSelectedClass))
				{
					classNames[i] = nameOfSelectedClass + " " + (level + 1);
				}
			}

			for (int i = 0; i < classNames.Length; i++)
			{
				character.CharacterClass.Name += classNames[i];
				if (i < classNames.Length - 1)
					character.CharacterClass.Name += " / ";
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
	}
}
