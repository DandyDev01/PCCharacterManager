using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClass
	{
		private string name;
		private HitDie hitDie;
		private DnD5eCharacterClassLevel level;

		[JsonProperty("HitDie")]
		[JsonConverter(typeof(StringEnumConverter))]
		public HitDie HitDie
		{
			get { return hitDie; }
			set
			{
				hitDie = value;
			}
		}
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
			}
		}
		public DnD5eCharacterClassLevel Level
		{
			get { return level; }
			set
			{
				level = value;
			}
		}

		public ObservableCollection<DnD5eCharacterClassFeature> Features
		{
			get;
			set;
		}

		public DnD5eCharacterClass() 
		{
			name = string.Empty;
			level = new DnD5eCharacterClassLevel();
			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}

		public DnD5eCharacterClass(DnD5eCharacterClassData data)
		{
			name = data.Name;
			hitDie = data.HitDie;
			level = data.Level;
			level.Level = 1;

			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}
	}
}
