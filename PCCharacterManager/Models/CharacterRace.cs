using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterRace
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public CharacterRaceVariant RaceVariant { get; set; }
		public List<Property> Features { get; set; }

		public CharacterRace() { }

		public CharacterRace(CharacterRaceData data)
		{
			Name = data.Name;
			Age = data.Age;
			RaceVariant = data.RaceVariant;
			Features = data.Features;
		}
	}
}
