using System.Collections.ObjectModel;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterRaceVariant
	{
		public DnD5eCharacterRaceVariant(string name, ObservableCollection<Property> properties)
		{
			Name = name;
			Properties = properties;
		}

		public DnD5eCharacterRaceVariant()
		{
			Name = "Name";
			Properties = new ObservableCollection<Property>();
		}

		public string Name { get; set; }
		public ObservableCollection<Property> Properties { get; set; }
	}
}
