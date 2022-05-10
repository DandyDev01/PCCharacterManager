using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PCCharacterManager.Models
{
	public class CharacterRaceData
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

		public CharacterRaceVariant RaceVariant { get; set; }

		public List<Property> Features { get; set; }
		public List<CharacterRaceVariant> Variants { get; set; }

		public CharacterRaceData()
		{
			Name = string.Empty;
			Speed = string.Empty;
			AgeRange = string.Empty;
			RaceVariant = new CharacterRaceVariant();
			Features = Array.Empty<Property>().ToList();
			Variants = Array.Empty<CharacterRaceVariant>().ToList();

			AbilityScoreIncreases = Array.Empty<string>();
			Languages = Array.Empty<string>();
			ArmorProficiencies = Array.Empty<string>();
			WeaponProficiencies = Array.Empty<string>();
			ToolProficiences = Array.Empty<string>();
		}
	}
}
