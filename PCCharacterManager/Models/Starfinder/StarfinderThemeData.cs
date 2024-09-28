using System.Collections.Generic;

namespace PCCharacterManager.Models
{
	public class StarfinderThemeData
	{
		public string Name { get; set; }
		public string AbilityScoreImprovement { get; set; }
		public List<DnD5eCharacterClassFeature> Features { get; set; }

		public StarfinderThemeData()
		{
			Features = new List<DnD5eCharacterClassFeature>();
			Name = string.Empty;
			AbilityScoreImprovement = string.Empty;
		}
	}
}
