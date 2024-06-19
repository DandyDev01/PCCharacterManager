using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PCCharacterManager.Models.Levelers
{
	public class DarkSoulsCharacterLeveler : CharacterLeveler
	{
		public DarkSoulsCharacterLeveler(DialogServiceBase dialogService) : base(dialogService)
		{
		}

		public override bool LevelCharacter(CharacterBase character)
		{
			if (CharacterTypeHelper.IsValidCharacterType(character, CharacterType.dark_souls) == false)
				return false;

			string className = character.CharacterClass.Name.Substring(0, character.CharacterClass.Name.IndexOf(" ")).Trim();

			if (UnlockFeatures(character as DarkSoulsCharacter, className) == false)
				return false;

			if (UpdateMaxHealth(character as DarkSoulsCharacter) == false)
				return false;

			character.Level.LevelUp();

			character.CharacterClass.Name = className + " " + character.Level.Level;

			return true;
		}

		private static bool UnlockFeatures(CharacterBase character, string className)
		{
			DnD5eCharacterClass[] classes = ReadWriteJsonCollection<DnD5eCharacterClass>.ReadCollection(
							DarkSoulsResources.CharacterClassDataJson).ToArray();

			if (classes.Length <= 0 || classes is null)
				return false;

			DnD5eCharacterClass classBeingLeveled = classes.First(x => x.Name.Equals(className));

			if (classBeingLeveled is null)
				return false;

			DnD5eCharacterClassFeature[] featuresToAdd = classBeingLeveled.Features.Where(
				x => x.Level == character.Level.Level + 1).ToArray();

			if (featuresToAdd.Length <= 0 || featuresToAdd is null)
				return false;

			foreach (var item in featuresToAdd)
			{
				character.CharacterClass.Features.Add(item);
			}

			return true;
		}

		/// <summary>
		/// Update the max health of a character.
		/// </summary>
		/// <param name="character">The character to update the max health of.</param>
		/// <returns></returns>
		private bool UpdateMaxHealth(DarkSoulsCharacter character)
		{
			var message = _dialogService.ShowMessage("Would you like to manually enter a new max health",
				"", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (message == MessageBoxResult.Yes)
			{
				return ManuallyEnterNewMaxHealth(character);
			}
			else if (message == MessageBoxResult.No)
			{
				return AutoRollNewMaxHealth(character);
			}
			else if (message == MessageBoxResult.Cancel)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Calculate a new max health for the character.
		/// </summary>
		/// <param name="character">Character being leveled up</param>
		/// <param name="helper">Information about the class being leveled up.</param>
		/// <returns>Information about the levelup operation.</returns>
		private bool AutoRollNewMaxHealth(DarkSoulsCharacter character)
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(character.Origin.HitDie, 1);
			int currHealth = character.Health.MaxHealth;
			int conMod = character.Abilities.First(x => x.Name.ToLower().Equals("constitution")).Modifier;

			character.Health.SetMaxHealth(currHealth + numToAddToHealth + conMod + character.Level.Level + 1);

			return true;
		}

		/// <summary>
		/// User enters a new max health for their character.
		/// </summary>
		/// <param name="character">The character that is being leveledup.</param>
		/// <param name="helper">Information about the class being leveled up.</param>
		/// <returns>Information about the levelup operation.</returns>
		private bool ManuallyEnterNewMaxHealth(DarkSoulsCharacter character)
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
				return false;

			int amount = int.Parse(windowVM.Answer);
			character.Health.SetMaxHealth(amount);

			return true;
		}
	}
}
