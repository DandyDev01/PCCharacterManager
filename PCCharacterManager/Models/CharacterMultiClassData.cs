using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
