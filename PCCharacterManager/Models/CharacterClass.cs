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
	public class CharacterClass
	{
		private string name;
		private HitDie hitDie;
		private CharacterClassLevel level;

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
		public CharacterClassLevel Level
		{
			get { return level; }
			set
			{
				level = value;
			}
		}

		public ObservableCollection<CharacterClassFeature> Features
		{
			get;
			set;
		}

		public CharacterClass() { }

		public CharacterClass(CharacterClassData data)
		{
			name = data.Name;
			hitDie = data.HitDie;
			level = data.Level;
			level.Level = 1;

			Features = new ObservableCollection<CharacterClassFeature>();
		}
	}
}
