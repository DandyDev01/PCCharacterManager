using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public enum SpellSchool { ALL, CONJURATION, NECROMANCY, EVOCATION, ABJURATION, TRANSMUTATION, DIVINATION, ENCHANTMENT, ILLUSION };

	public class Spell : Item
	{
		private string level = "1"; // can be a max of 9 regular 5e
		private string castingTime = "1 Action";
		private string range_Area;
		private string damage_Effect;
		private string attack_Save;
		private SpellSchool school;
		private string duration;
		private bool isPrepared = false;

		public string Level
		{
			get { return level; }
			set { OnPropertyChanged(ref level, value); }
		}
		public string CastingTime
		{
			get { return castingTime; }
			set { OnPropertyChanged(ref castingTime, value); }
		}
		public string Range_Area
		{
			get { return range_Area; }
			set { OnPropertyChanged(ref range_Area, value); }
		}
		public string Damage_Effect
		{
			get { return damage_Effect; }
			set { OnPropertyChanged(ref damage_Effect, value); }
		}
		public string Attack_Save
		{
			get { return attack_Save; }
			set { OnPropertyChanged(ref attack_Save, value); }
		}
		[Newtonsoft.Json.JsonProperty("School")]
		public SpellSchool School
		{
			get { return school; }
			set { OnPropertyChanged(ref school, value); }
		}
		public string Duration
		{
			get { return duration; }
			set { OnPropertyChanged(ref duration, value); }
		}
		public bool IsPrepared
		{
			get { return isPrepared; }
			set { OnPropertyChanged(ref isPrepared, value); }
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
