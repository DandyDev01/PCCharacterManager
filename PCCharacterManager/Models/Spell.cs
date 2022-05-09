using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Spell : Item
	{
		private string level = "1"; // can be a max of 9 regular 5e
		private string castingTime = "1 Action";
		private string range_Area;
		private string damage_Effect;
		private string attack_Save;
		private string school;
		private string duration;
		private bool isPrepared = false;

		public string Level
		{
			get { return level; }
			set { OnPropertyChaged(ref level, value); }
		}
		public string CastingTime
		{
			get { return castingTime; }
			set { OnPropertyChaged(ref castingTime, value); }
		}
		public string Range_Area
		{
			get { return range_Area; }
			set { OnPropertyChaged(ref range_Area, value); }
		}
		public string Damage_Effect
		{
			get { return damage_Effect; }
			set { OnPropertyChaged(ref damage_Effect, value); }
		}
		public string Attack_Save
		{
			get { return attack_Save; }
			set { OnPropertyChaged(ref attack_Save, value); }
		}
		public string School
		{
			get { return school; }
			set { OnPropertyChaged(ref school, value); }
		}
		public string Duration
		{
			get { return duration; }
			set { OnPropertyChaged(ref duration, value); }
		}
		public bool IsPrepared
		{
			get { return isPrepared; }
			set { OnPropertyChaged(ref isPrepared, value); }
		}
		// physical requirements needed to cast the spell
		// Verbal (V), Somatic (S) or Material (M)
		public ObservableCollection<char> Components { get; set; }
		public string StringComponents
		{
			get
			{
				var builder = new StringBuilder();

				if (Components == null)
					return null;

				foreach (var item in Components)
				{
					builder.Append(item.ToString());
				}
				return builder.ToString();
			}
		}
	}
}
