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
	public enum SpellSchool 
	{ 
		ALL, CONJURATION, NECROMANCY, EVOCATION, ABJURATION, 
		TRANSMUTATION, DIVINATION, ENCHANTMENT, ILLUSION 
	};

	public class Spell : Item
	{
		private string level = "1"; // can be a max of 9 regular 5e
		public string Level
		{
			get { return level; }
			set { OnPropertyChanged(ref level, value); }
		}
		
		private string castingTime = "1 Action";
		public string CastingTime
		{
			get { return castingTime; }
			set { OnPropertyChanged(ref castingTime, value); }
		}
		
		private string range_Area;
		public string Range_Area
		{
			get { return range_Area; }
			set { OnPropertyChanged(ref range_Area, value); }
		}
		
		private string damage_Effect;
		public string Damage_Effect
		{
			get { return damage_Effect; }
			set { OnPropertyChanged(ref damage_Effect, value); }
		}
		
		private string attack_Save;
		public string Attack_Save
		{
			get { return attack_Save; }
			set { OnPropertyChanged(ref attack_Save, value); }
		}
		
		private string duration;
		public string Duration
		{
			get { return duration; }
			set { OnPropertyChanged(ref duration, value); }
		}
		
		private bool isPrepared = false;
		public bool IsPrepared
		{
			get { return isPrepared; }
			set { OnPropertyChanged(ref isPrepared, value); }
		}
		
		private SpellSchool school;
		[JsonProperty("School")]
		[JsonConverter(typeof(StringEnumConverter))]
		public SpellSchool School
		{
			get { return school; }
			set { OnPropertyChanged(ref school, value); }
		}

		// physical requirements needed to cast the spell
		// Verbal (V), Somatic (S) or Material (M)
		
		public ObservableCollection<char> Components { get; set; }
		
		public string StringComponents
		{
			get
			{
				var builder = new StringBuilder();

				foreach (var item in Components)
				{
					builder.Append(item.ToString());
				}
				return builder.ToString();
			}
		}

		public Spell()
		{
			level = string.Empty;
			level = string.Empty;
			castingTime = string.Empty;
			range_Area = string.Empty;
			damage_Effect = string.Empty;
			attack_Save = string.Empty;
			school = SpellSchool.EVOCATION;
			duration = string.Empty;
			Components = new ObservableCollection<char>();
		}
	}
}
