using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterRaceVariant
	{
		public CharacterRaceVariant(string name, List<Property> properties)
		{
			Name = name;
			Properties = properties;
		}

		public CharacterRaceVariant()
		{
			Name = "Name";
			Properties = new List<Property>();
		}

		public string Name { get; set; }
		public List<Property> Properties { get; set; }
	}
}
