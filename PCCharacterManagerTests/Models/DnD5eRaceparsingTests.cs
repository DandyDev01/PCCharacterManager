using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class DnD5eRaceparsingTests
	{
		DnD5eCharacterRaceData[] races = ReadWriteJsonCollection<DnD5eCharacterRaceData>.ReadCollection(DnD5eResources.RaceDataJson).ToArray();
		Ability[] abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

		[TestMethod()]
		public void AbilityScoreIncreasesTest()
		{
			bool anyFailed = false;

			foreach (var race in races)
			{
				string[] abilityScoreIncreases = race.AbilityScoreIncreases;

				foreach (string abiltyScoreIncrease in abilityScoreIncreases)
				{
					Regex regex = new Regex("x+[0-9]");
					int index = regex.Match(abiltyScoreIncrease).Index != 0 
						? regex.Match(abiltyScoreIncrease).Index : abiltyScoreIncrease.Length;
					string abiliytName = abiltyScoreIncrease.Substring(0, index);

					if (abiliytName.Contains("Your Choice", StringComparison.OrdinalIgnoreCase))
						continue;

					anyFailed = Check(anyFailed, race, abiliytName.Substring(0, index).Trim());
				}
			}

			Assert.IsFalse(anyFailed);
		}

		[TestMethod()]
		public void VariantAbilityScoreIncreasesTest()
		{
			bool anyFailed = false;

			foreach (var race in races)
			{
				Property[] raceVarientPropertys = race.RaceVariant.Properties.ToArray();

				foreach (Property property in raceVarientPropertys)
				{
					if (property.Name.Contains("Ability Score Increase", StringComparison.OrdinalIgnoreCase))
						continue;

					string ability = property.Desc.Substring(0, property.Desc.IndexOf(" "));

					anyFailed = Check(anyFailed, race, ability.Trim());
				}
			}

			Assert.IsFalse(anyFailed);
		}

		private bool Check(bool anyFailed, DnD5eCharacterRaceData race, string abilityName)
		{
			try
			{
				Ability ability = abilities.Where(y => y.Name == abilityName).First();
				Assert.IsNotNull(ability);
			}
			catch (Exception ex)
			{
				anyFailed = true;
				Trace.WriteLine("Failed Race: " + race.Name);
				Trace.WriteLine("Failed Ability: " + abilityName);
			}

			return anyFailed;
		}
	}
}
