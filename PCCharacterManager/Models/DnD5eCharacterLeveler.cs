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
		protected override bool UpdateMaxHealth(DnD5eCharacter character)
		{
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
					return false;

				int amount = int.Parse(windowVM.Answer);
				character.Health.SetMaxHealth(amount);

				return true;
			}
			else if (message == MessageBoxResult.No)
			{
				string classHitDie = character.CharacterClass.HitDie.ToString();

				int numRolls = 1;
				int sides = (int.Parse(classHitDie.Substring(classHitDie.IndexOf('D') + 1)));

				RollDie rollDie = new RollDie();

				int numToAddToHealth = rollDie.Roll(sides, numRolls);
				int currHealth = character.Health.MaxHealth;

				character.Health.SetMaxHealth(currHealth + numToAddToHealth);

				return true;
			}
			else if (message == MessageBoxResult.Cancel)
			{
				return false;
			}

			return false;
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
				var classes = ReadWriteJsonCollection<DnD5eCharacter>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
				string[] classNames = new string[classes.Length];

				for (int i = 0; i < classes.Length; i++)
				{
					classNames[i] = classes[i].Name;
				}

				Window selectClassWindow = new SelectStringValueDialogWindow();
				DialogWindowSelectStingValue vm = 
					new DialogWindowSelectStingValue(selectClassWindow, classNames, 1);

				string nameOfSelectedClass = vm.SelectedItems.First();

				string currentClassName = character.CharacterClass.Name;
				character.CharacterClass.Name = currentClassName + character.CharacterClass.Level.Level + 
					" / " + nameOfSelectedClass + 1;

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
