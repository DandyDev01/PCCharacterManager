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

		}

		[TestMethod()]
		public void SetSaveTest()
		{

		}

		[TestMethod()]
		public void SetModTest()
		{

		}

		[TestMethod()]
		public void SetProfSaveTest()
		{

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
