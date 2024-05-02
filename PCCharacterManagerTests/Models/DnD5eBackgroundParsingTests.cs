using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class DnD5eBackgroundParsingTests
	{
		DnD5eBackgroundData[] backgrounds = ReadWriteJsonCollection<DnD5eBackgroundData>.ReadCollection(DnD5eResources.BackgroundDataJson).ToArray();
		Ability[] abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

		[TestMethod()]
		public void AllBackgroundsTest()
		{
			bool anyFailed = false;

			foreach (var background in backgrounds)
			{
				string[] skillProfs = background.SkillProfs;

				foreach (string skillName in skillProfs)
				{
					if (skillName == "1 of your choice")
						continue;

					if (skillName.Contains("^"))
					{
						string[] otherSkills = skillName.Split("^");
						foreach (string skill in otherSkills)
						{
							anyFailed = Check(anyFailed, background, skill.Trim());
						}
					}
					else
					{
						anyFailed = Check(anyFailed, background, skillName);
					}
				}
			}

			Assert.IsFalse(anyFailed);
		}

		private bool Check(bool anyFailed, DnD5eBackgroundData background, string skillName)
		{
			try
			{
				Ability skillOwner = abilities.Where(x => x.Skills.Any(y => y.Name == skillName)).First();
				Assert.IsNotNull(skillOwner);
			}
			catch (Exception ex)
			{
				anyFailed = true;
				Trace.WriteLine("Failed Background: " + background.Name);
				Trace.WriteLine("Failed Skill: " + skillName);
			}

			return anyFailed;
		}
	}
}
