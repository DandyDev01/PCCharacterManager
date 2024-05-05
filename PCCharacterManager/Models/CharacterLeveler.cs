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
			var addClassHelper = AddClass(character);

			if (addClassHelper.didAddClass)
			{

				UnLockClassFeatures(character, addClassHelper.newClassName, 1);
			}
			else
			{
				if (UpdateMaxHealth(character) == false)
					return;

				character.CharacterClass.Level.LevelUp();
				UnLockClassFeatures(character, "", 0);

			}
			
			character.Level.LevelUp();
			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}
		}

		protected abstract AddClassHelper AddClass(DnD5eCharacter character);

		protected abstract bool UpdateMaxHealth(DnD5eCharacter character);

		protected abstract void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel);
	}

	public struct AddClassHelper
	{
		public bool didAddClass;
		public string newClassName;
	}
}
