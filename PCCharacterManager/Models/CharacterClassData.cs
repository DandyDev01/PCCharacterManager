using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterClassData
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
		public CharacterClassFeature[] Features
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

		public CharacterClassData()
		{
			name = string.Empty;
			hitDie = HitDie.D4;
			level = new CharacterClassLevel();
			ArmorProficiencies = Array.Empty<string>();
			WeaponProficiencies = Array.Empty<string>();
			ToolProficiencies = Array.Empty<string>();
			SavingThrows = Array.Empty<string>();
			StartEquipment = Array.Empty<string>();
			PossibleSkillProficiences = Array.Empty<string>();
			Features = Array.Empty<CharacterClassFeature>();
		}
	}
}
