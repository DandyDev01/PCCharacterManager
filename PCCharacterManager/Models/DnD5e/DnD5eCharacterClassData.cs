using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClassData
	{
		private string _name;
		private HitDie _hitDie;
		private DnD5eCharacterClassLevel _level;

		[JsonProperty("HitDie")]
		[JsonConverter(typeof(StringEnumConverter))]
		public HitDie HitDie
		{
			get { return _hitDie; }
			set
			{
				_hitDie = value;
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
		public string[] ArmorProficiencies
		{
			get;
			set;
		}
		public string[] WeaponProficiencies
		{
			get;
			set;
		}
		public string[] ToolProficiencies
		{
			get;
			set;
		}
		public string[] PossibleSkillProficiences
		{
			get;
			set;
		}
		public DnD5eCharacterClassFeature[] Features
		{
			get;
			set;
		}

		public string[] SavingThrows { get; set; }

		public string[] StartEquipment { get; set; }

		/// <summary>
		/// the number of skill proficienes a class can have
		/// </summary>
		public int NumOfSkillProficiences
		{
			get { return numOfSkillProficiences; }
			set { numOfSkillProficiences = value; }
		}
		private int numOfSkillProficiences;

		public DnD5eCharacterClassData()
		{
			_name = string.Empty;
			_hitDie = HitDie.D4;
			_level = new DnD5eCharacterClassLevel();
			ArmorProficiencies = Array.Empty<string>();
			WeaponProficiencies = Array.Empty<string>();
			ToolProficiencies = Array.Empty<string>();
			SavingThrows = Array.Empty<string>();
			StartEquipment = Array.Empty<string>();
			PossibleSkillProficiences = Array.Empty<string>();
			Features = Array.Empty<DnD5eCharacterClassFeature>();
		}
	}
}
