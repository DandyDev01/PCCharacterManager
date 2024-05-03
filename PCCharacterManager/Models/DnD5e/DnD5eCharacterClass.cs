using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClass : ObservableObject
	{
		private string _name;
		private HitDie _hitDie;
		private DnD5eCharacterClassLevel _level;

		[JsonProperty(nameof(HitDie))]
		[JsonConverter(typeof(StringEnumConverter))]
		public HitDie HitDie
		{
			get { return _hitDie; }
			set
			{
				OnPropertyChanged(ref _hitDie, value);
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
			}
		}
		public DnD5eCharacterClassLevel Level
		{
			get { return _level; }
			set
			{
				_level = value;
			}
		}

		public ObservableCollection<DnD5eCharacterClassFeature> Features
		{
			get;
			set;
		}

		public DnD5eCharacterClass() 
		{
			_name = string.Empty;
			_level = new DnD5eCharacterClassLevel();
			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}

		public DnD5eCharacterClass(DnD5eCharacterClassData data)
		{
			_name = data.Name;
			_hitDie = data.HitDie;
			_level = data.Level;
			_level.Level = 1;

			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}
	}
}
