using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class DnD5eMultiClassDataTests
	{
		DnD5eCharacterClassData[] classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
		CharacterMultiClassData[] multiClassData = ReadWriteJsonCollection<CharacterMultiClassData>.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray();
		Ability[] abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

		[TestMethod()]
		public void AllDataTest()
		{
			string[] skills = Ability.GetSkillNames().ToArray();
			bool anyFail = false;

			foreach (var classData in multiClassData)
			{
				DnD5eCharacterClassData currentClass = classes.Where(x => x.Name == classData.Name).First();

				if (currentClass is null)
				{
					Trace.WriteLine("Failed class: " + classData.Name);
					anyFail = true;
					continue;
				}

				if (classData.PossibleSkillProficiences is null)
					classData.PossibleSkillProficiences = new string[0];

				foreach (var skill in classData.PossibleSkillProficiences)
				{
					if (skills.Contains(skill) == false)
					{
						anyFail = true;
						Trace.WriteLine("Failed Skill: " + skill + " for " + classData.Name);
					}
				}

				if (classData.ArmorProficiencies is null)
					classData.ArmorProficiencies = new string[0];

				foreach (var armorProf in classData.ArmorProficiencies)
				{
					if (currentClass.ArmorProficiencies.Contains(armorProf) == false)
					{
						anyFail = true;
						Trace.WriteLine("Failed armor prof: " + armorProf + " for " + classData.Name);
					}
				}

				if (classData.WeaponProficiencies is null)
					classData.WeaponProficiencies = new string[0];

				foreach (var weaponProf in classData.WeaponProficiencies)
				{
					if (currentClass.WeaponProficiencies.Contains(weaponProf) == false)
					{
						anyFail = true;
						Trace.WriteLine("Failed weapon prof: " + weaponProf + " for " + classData.Name);
					}
				}

				if (classData.ToolProficiences is null)
					classData.ToolProficiences = new string[0];

				foreach (var toolProf in classData.ToolProficiences)
				{
					// special case
					if (classData.Name == "Bard" && toolProf == "one musical intrument of your choice")
						continue;

					if (currentClass.ToolProficiencies.Contains(toolProf) == false)
					{
						anyFail = true;
						Trace.WriteLine("Failed tool prof: " + toolProf + " for " + classData.Name);
					}
				}

				string[] prerequisites = classData.Prerequisites.Split('^', '&');
				for (int i = 0; i < prerequisites.Length; i++)
				{
					prerequisites[i] = prerequisites[i].Substring(0, prerequisites[i].IndexOf(" ")).Trim();
					if (prerequisites[i] == string.Empty)
						continue;
					if (abilities.Any(x => x.Name == prerequisites[i]) == false)
					{
						anyFail = true;
						Trace.WriteLine("Failed prerequisite: " + prerequisites[i] + " for " + classData.Name);
					}
				}
			}

			Assert.IsFalse(anyFail);
		}
	}
}
