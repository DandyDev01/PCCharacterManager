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

		protected override void UnLockClassFeatures(DnD5eCharacter character)
		{
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);

			// find character class
			DnD5eCharacterClassData? data = classes.Find(x => x.Name.Equals(character.CharacterClass.Name));

			if (data == null)
				throw new Exception("The class " + character.CharacterClass.Name + " does not exist");

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
	}
}
