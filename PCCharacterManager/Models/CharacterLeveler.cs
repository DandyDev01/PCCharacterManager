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
				MultiClass helper = UpdateMaxHealth(character);
				if (helper.success == false)
					return;

				character.CharacterClass.Level.LevelUp();
				UnLockClassFeatures(character, helper.className, helper.classLevel);

			}
			
			character.Level.LevelUp();
			foreach (var ability in character.Abilities)
			{
				ability.SetProfBonus(character.Level.ProficiencyBonus);
			}
		}

		protected abstract AddClassHelper AddClass(DnD5eCharacter character);

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
