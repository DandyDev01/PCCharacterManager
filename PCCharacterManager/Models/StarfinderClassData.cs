using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderClassData : DnD5eCharacterClassData
	{
		public int HitPoints { get; set; }
		public int StaminaPoints { get; set; }

		/// <summary>
		/// seperare options with either ^ (or) or &(and)
		/// </summary>
		public string KeyAbilityScore { get; set; }
		public string SkillPointsAtEachLevel { get; set; }
		public string[] ClassSkills { get; set; }

		public StarfinderClassData()
		{
			KeyAbilityScore = string.Empty;
			SkillPointsAtEachLevel = string.Empty;
			ClassSkills = Array.Empty<string>();
			Features = Array.Empty<DnD5eCharacterClassFeature>();
		}
	}
}
