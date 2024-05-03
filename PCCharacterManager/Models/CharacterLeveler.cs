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
			if (UpdateMaxHealth(character) == false)
				return;

			character.Level.LevelUp();
			character.CharacterClass.Level.LevelUp();
			UnLockClassFeatures(character);

			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}
		}

		protected abstract bool UpdateMaxHealth(DnD5eCharacter character);

		protected abstract void UnLockClassFeatures(DnD5eCharacter character);
	}
}
