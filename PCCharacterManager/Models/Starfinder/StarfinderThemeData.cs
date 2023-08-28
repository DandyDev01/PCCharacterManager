using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderThemeData
	{
		public string Name { get; set; }
		public string AbilityScoreImprovement { get; set; }
		public List<DnD5eCharacterClassFeature> Features { get; set; }

		StarfinderThemeData()
		{
			Features = new List<DnD5eCharacterClassFeature>();
			Name = string.Empty;
			AbilityScoreImprovement = string.Empty;
		}
	}
}
