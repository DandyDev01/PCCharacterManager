using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterRaceVariant
	{
		public DnD5eCharacterRaceVariant(string name, List<Property> properties)
		{
			Name = name;
			Properties = properties;
		}

		public DnD5eCharacterRaceVariant()
		{
			Name = "Name";
			Properties = new List<Property>();
		}

		public string Name { get; set; }
		public List<Property> Properties { get; set; }
	}
}
