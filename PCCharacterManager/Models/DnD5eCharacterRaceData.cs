using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterRaceData
	{
		public string Name { get; set; }

		public string Speed { get; set; }
		[JsonProperty("Size")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }
		public int Age { get; set; }
		public string[] AbilityScoreIncreases { get; set; }
		public string[] Languages { get; set; }
		public string AgeRange { get; set; }
		public string[] ArmorProficiencies { get; set; }
		public string[] WeaponProficiencies { get; set; }
		public string[] ToolProficiences { get; set; }

		public DnD5eCharacterRaceVariant RaceVariant { get; set; }

		public List<Property> Features { get; set; }
		public List<DnD5eCharacterRaceVariant> Variants { get; set; }

		public DnD5eCharacterRaceData()
		{
			Name = string.Empty;
			Speed = string.Empty;
			AgeRange = string.Empty;
			RaceVariant = new DnD5eCharacterRaceVariant();
			Features = Array.Empty<Property>().ToList();
			Variants = Array.Empty<DnD5eCharacterRaceVariant>().ToList();

			AbilityScoreIncreases = Array.Empty<string>();
			Languages = Array.Empty<string>();
			ArmorProficiencies = Array.Empty<string>();
			WeaponProficiencies = Array.Empty<string>();
			ToolProficiences = Array.Empty<string>();
		}
	}
}
