using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterRace
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public DnD5eCharacterRaceVariant RaceVariant { get; set; }
		public ObservableCollection<Property> Features { get; set; }

		public DnD5eCharacterRace() 
		{
			Name = string.Empty;
			RaceVariant = new DnD5eCharacterRaceVariant();
			Features = new ObservableCollection<Property>();	
		}

		public DnD5eCharacterRace(DnD5eCharacterRaceData data)
		{
			Name = data.Name;
			Age = data.Age;
			RaceVariant = data.RaceVariant;
			Features = new ObservableCollection<Property>();
			foreach (var item in data.Features)
			{
				Features.Add(item);
			}
		}
	}
}
