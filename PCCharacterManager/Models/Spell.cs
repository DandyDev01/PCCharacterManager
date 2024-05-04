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
		private string _level = "1"; // can be a max of 9 regular 5e
		public string Level
		{
			get { return _level; }
			set { OnPropertyChanged(ref _level, value); }
		}
		
		private string _castingTime = "1 Action";
		public string CastingTime
		{
			get { return _castingTime; }
			set { OnPropertyChanged(ref _castingTime, value); }
		}
		
		private string _range_Area;
		public string Range_Area
		{
			get { return _range_Area; }
			set { OnPropertyChanged(ref _range_Area, value); }
		}
		
		private string _damage_Effect;
		public string Damage_Effect
		{
			get { return _damage_Effect; }
			set { OnPropertyChanged(ref _damage_Effect, value); }
		}
		
		private string _attack_Save;
		public string Attack_Save
		{
			get { return _attack_Save; }
			set { OnPropertyChanged(ref _attack_Save, value); }
		}
		
		private string _duration;
		public string Duration
		{
			get { return _duration; }
			set { OnPropertyChanged(ref _duration, value); }
		}
		
		private bool _isPrepared = false;
		public bool IsPrepared
		{
			get { return _isPrepared; }
			set { OnPropertyChanged(ref _isPrepared, value); }
		}
		
		private SpellSchool _school;
		[JsonProperty("School")]
		[JsonConverter(typeof(StringEnumConverter))]
		public SpellSchool School
		{
			get { return _school; }
			set { OnPropertyChanged(ref _school, value); }
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
			_level = string.Empty;
			_level = string.Empty;
			_castingTime = string.Empty;
			_range_Area = string.Empty;
			_damage_Effect = string.Empty;
			_attack_Save = string.Empty;
			_school = SpellSchool.EVOCATION;
			_duration = string.Empty;
			Components = new ObservableCollection<char>();
		}
	}
}
