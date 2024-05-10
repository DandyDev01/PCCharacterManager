using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class AbilityTests
	{
		[TestMethod()]
		public void SetProfBonusTest()
		{
			var skill = new AbilitySkill();

			skill.SkillProficiency = true;

			AbilitySkill[] skills = {
				skill
			};
			Ability ability = new Ability(20, 2, 1, false, 2, "ability", "none", skills);

			ability.SetProfBonus(1);
			foreach (var item in ability.Skills)
			{
				Assert.IsTrue(item.Score == 4);
			}

			ability.SetProfBonus(3);
			foreach (var item in ability.Skills)
			{
				Assert.IsTrue(item.Score == 5);
			}

			skill.SkillProficiency = false;
			ability.SetProfBonus(3);
			foreach (var item in ability.Skills)
			{
				Assert.IsTrue(item.Score == 2);
			}
		}

		[TestMethod()]
		public void SetSaveTest()
		{
			
		}

		[TestMethod()]
		public void SetModTest()
		{
			Ability ability = new Ability(20, 2, 1, false, 2, "ability", "none", null);
			
			ability.Score = 1;
			Assert.IsTrue(ability.Modifier == -5);
			ability.Score = 2;
			Assert.IsTrue(ability.Modifier == -4);
			ability.Score = 3;
			Assert.IsTrue(ability.Modifier == -4);
			ability.Score = 4;
			Assert.IsTrue(ability.Modifier == -3);
			ability.Score = 5;
			Assert.IsTrue(ability.Modifier == -3);
			ability.Score = 6;
			Assert.IsTrue(ability.Modifier == -2);
			ability.Score = 7;
			Assert.IsTrue(ability.Modifier == -2);
			ability.Score = 8;
			Assert.IsTrue(ability.Modifier == -1);
			ability.Score = 9;
			Assert.IsTrue(ability.Modifier == -1);
			ability.Score = 10;
			Assert.IsTrue(ability.Modifier == 0);

			ability.Score = 11;
			Assert.IsTrue(ability.Modifier == 0);
			ability.Score = 12;
			Assert.IsTrue(ability.Modifier == 1);
			ability.Score = 13;
			Assert.IsTrue(ability.Modifier == 1);
			ability.Score = 14;
			Assert.IsTrue(ability.Modifier == 2);
			ability.Score = 15;
			Assert.IsTrue(ability.Modifier == 2);
			ability.Score = 16;
			Assert.IsTrue(ability.Modifier == 3);
			ability.Score = 17;
			Assert.IsTrue(ability.Modifier == 3);
			ability.Score = 18;
			Assert.IsTrue(ability.Modifier == 4);
			ability.Score = 19;
			Assert.IsTrue(ability.Modifier == 4);
			ability.Score = 20;
			Assert.IsTrue(ability.Modifier == 5);

			ability.Score = 21;
			Assert.IsTrue(ability.Modifier == 5);
			ability.Score = 22;
			Assert.IsTrue(ability.Modifier == 6);
			ability.Score = 23;
			Assert.IsTrue(ability.Modifier == 6);
			ability.Score = 24;
			Assert.IsTrue(ability.Modifier == 7);
			ability.Score = 25;
			Assert.IsTrue(ability.Modifier == 7);
			ability.Score = 26;
			Assert.IsTrue(ability.Modifier == 8);
			ability.Score = 27;
			Assert.IsTrue(ability.Modifier == 8);
			ability.Score = 28;
			Assert.IsTrue(ability.Modifier == 9);
			ability.Score = 29;
			Assert.IsTrue(ability.Modifier == 9);
			ability.Score = 30;
			Assert.IsTrue(ability.Modifier == 10);

			ability.Score = -1;
			Assert.IsTrue(ability.Modifier == -5);
		}

		[TestMethod()]
		public void SetProfSaveTest()
		{
			Ability ability = new Ability();
			Assert.IsFalse(ability.ProfSave);
			
			ability.ProfSave = true;
			Assert.IsTrue(ability.ProfSave);
		}

		[TestMethod()]
		public void FindAbilityTests()
		{
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson);
			AbilitySkill skill = abilities[0].Skills[0];

			Assert.IsTrue(Ability.FindAbility(abilities.ToArray(), skill) != null);
		}

		[TestMethod()]
		public void FindSkillTest()
		{
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson);
			string[] skillNames = {
				"Athletics",
				"Acrobatics",
				"Sleight of Hand",
				"Stealth",
				"Arcana",
				"History",
				"Investigation",
				"Nature",
				"Religion",
				"Animal Handling",
				"Insight",
				"Medicine",
				"Perception",
				"Survival",
				"Deception",
				"Intimidation",
				"Performance",
				"Persuasion"
			};

			bool allTrue = true;
			foreach (string skillName in skillNames)
			{
				AbilitySkill skill = Ability.FindSkill(abilities.ToArray(), skillName);
				if (skill is null)
					allTrue = false;
			}

			Assert.IsTrue(allTrue);
		}

		[TestMethod()]
		public void GetSkillsTest()
		{
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson);
			string[] skillNames = {
				"Athletics",
				"Acrobatics",
				"Sleight of Hand",
				"Stealth",
				"Arcana",
				"History",
				"Investigation",
				"Nature",
				"Religion",
				"Animal Handling",
				"Insight",
				"Medicine",
				"Perception",
				"Survival",
				"Deception",
				"Intimidation",
				"Performance",
				"Persuasion"
			};

			var foundSkills = Ability.GetSkills(abilities.ToArray());
			bool allTrue = true;

			foreach (AbilitySkill skill in foundSkills)
			{
				if (skillNames.Contains(skill.Name) == false)
					allTrue = false;
			}

			Assert.IsTrue(allTrue);
		}

		[TestMethod()]
		public void GetAbilityNamesTest()
		{
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson);
			string[] abilityNames = {
				"Strength",
				"Dexterity",
				"Constitution",
				"Intelligence",
				"Wisdom",
				"Charisma",
			};

			bool allTrue = true;
			var foundNames = Ability.GetAbilityNames(abilities.ToArray());
			foreach (var foundName in foundNames)
			{
				if (abilityNames.Contains(foundName) == false)
					allTrue = false;
			}

			Assert.IsTrue(allTrue);
		}

		[TestMethod()]
		public void GetSkillNamesTest()
		{
			string[] skillNames = {
				"Athletics",
				"Acrobatics",
				"Sleight of Hand",
				"Stealth",
				"Arcana",
				"History",
				"Investigation",
				"Nature",
				"Religion",
				"Animal Handling",
				"Insight",
				"Medicine",
				"Perception",
				"Survival",
				"Deception",
				"Intimidation",
				"Performance",
				"Persuasion"
			};

			bool allPassed = true;

			var foundNames = Ability.GetSkillNames();
			foreach ( var skillName in foundNames)
			{
				if (skillNames.Contains(skillName) == false)
					allPassed = false;
			}

			Assert.IsTrue(allPassed);

		}

		[TestMethod()]
		public void GetProficientSkillNamesTest()
		{

		}
	}
}
