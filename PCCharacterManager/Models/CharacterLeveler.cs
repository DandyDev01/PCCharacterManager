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
	public class CharacterLeveler
	{
		public void LevelCharacter(DnD5eCharacter character)
		{
			UpdateMaxHealth(character);
			character.Level.LevelUp();
			character.CharacterClass.Level.LevelUp();
			UnLockClassFeatures(character);

			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}
		}

		private void UpdateMaxHealth(DnD5eCharacter character)
		{
			var message = MessageBox.Show("Would you like to manually enter a new max health",
				"", MessageBoxButton.YesNo);

			if (message == MessageBoxResult.Yes)
			{
				Window window = new StringInputDialogWindow();
				DialogWindowStringInputViewModel windowVM =
					new DialogWindowStringInputViewModel(window);
				window.DataContext = windowVM;
				window.ShowDialog();

				if (window.DialogResult == false) return;

				int amount = int.Parse(windowVM.Answer);
				character.Health.SetMaxHealth(amount);
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
			}
		}

		private void UnLockClassFeatures(DnD5eCharacter character)
		{
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);

			// find character class
			DnD5eCharacterClassData? data = classes.Find(x => x.Name.Equals(character.CharacterClass.Name));

			if (data == null) throw new Exception("The class " + character.CharacterClass.Name + " does not exist");

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
