using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterMultiClassData
	{
		public string Name;
		public string HitDie;
		public string[] ArmorProficiencies;
		public string[] WeaponProficiencies;
		public string[] ToolProficiences;
		public string[] PossibleSkillProficiences;
		public int numOfSkillProficiences;
		public string Prerequisites;

		public CharacterMultiClassData(string name, string hitDie, string[] armorProficiencies, 
			string[] toolProficiences, string[] possibleSkillProficiences, int numOfSkillProficiences,
			string prerequisites, string[] weaponProficiencies)
		{
			Name = name;
			HitDie = hitDie;
			ArmorProficiencies = armorProficiencies;
			ToolProficiences = toolProficiences;
			PossibleSkillProficiences = possibleSkillProficiences;
			this.numOfSkillProficiences = numOfSkillProficiences;
			Prerequisites = prerequisites;
			WeaponProficiencies = weaponProficiencies;
		}

		/// <summary>
		/// Adds the weapon, tool, and armor proficiences of the class to the character.
		/// </summary>
		/// <param name="character">Character to add the proficiencies too.</param>
		public void AddProficiences(DnD5eCharacter character)
		{
			character.ToolProficiences.AddRange(ToolProficiences.Where(x => character.ToolProficiences.Contains(x) == false));
			character.WeaponProficiencies.AddRange(WeaponProficiencies.Where(x => character.WeaponProficiencies.Contains(x) == false));
			character.ArmorProficiencies.AddRange(ArmorProficiencies.Where(x => character.ArmorProficiencies.Contains(x) == false));
		}

		public string[] GetGainedToolProficiences(DnD5eCharacter character)
		{
			return ToolProficiences.Where(x => character.ToolProficiences.Contains(x) == false).ToArray();
		}

		public string[] GetGainedWeaponProficiences(DnD5eCharacter character)
		{
			return WeaponProficiencies.Where(x => character.WeaponProficiencies.Contains(x) == false).ToArray();
		}

		public string[] GetGainedArmorProficiences(DnD5eCharacter character)
		{
			return ArmorProficiencies.Where(x => character.ArmorProficiencies.Contains(x) == false).ToArray();
		}

		/// <summary>
		/// Determines if the character meets the prerequisites for the class they want to multiclass in.
		/// </summary>
		/// <param name="character">Character that is being checked.</param>
		/// <param name="characterMultiClassData">Multiclass prerequisite data.</param>
		/// <returns></returns>
		public bool MeetsPrerequisites(DnD5eCharacter character)
		{
			KeyValuePair<string, int>[] prerequisites = GetPrerequisites();

			// prerequsite contains an OR
			if (Prerequisites.Contains('^'))
			{
				bool meetsOne = false;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i].Key).First();
					if (ability.Score >= prerequisites[i].Value)
						meetsOne = true;
				}

				if (meetsOne)
					return true;
			}
			// prerequisite contains an AND
			else if (Prerequisites.Contains('&'))
			{
				bool meetsAll = true;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i].Key).First();
					if (ability.Score < prerequisites[i].Value)
						meetsAll = false;
				}

				if (meetsAll)
					return true;
			}
			// there is only a single prerequisite 
			else
			{
				string abilityname = Prerequisites;
				int abilityScore = 0;
				if (int.TryParse(abilityname.Substring(abilityname.IndexOf(" ")).Trim(), out abilityScore) == false)
				{
					throw new Exception("Could not find score.");
				}
				abilityname = abilityname.Substring(0, abilityname.IndexOf(" ")).Trim();
				Ability a = character.Abilities.Where(x => x.Name == abilityname).First();

				if (a.Score >= abilityScore)
					return true;
			}

			return false;
		}

		public KeyValuePair<string, int>[] GetPrerequisites()
		{
			string[] prerequisites = Prerequisites.Split(StringConstants.OR, StringConstants.AND);
			int[] score = new int[prerequisites.Length];
			KeyValuePair<string, int>[] results = new KeyValuePair<string, int>[prerequisites.Length];

			// get ability prerequisite name and score.
			for (int i = 0; i < prerequisites.Length; i++)
			{
				prerequisites[i] = prerequisites[i].Trim();
				if (int.TryParse(prerequisites[i].Substring(prerequisites[i].IndexOf(" ")).Trim(), out score[i]) == false)
				{
					throw new Exception("Could not find score.");
				}
				prerequisites[i] = prerequisites[i].Substring(0, prerequisites[i].IndexOf(" ")).Trim();
				results[i] = new KeyValuePair<string, int>(prerequisites[i], score[i]);
			}

			return results;
		}
	}
}
