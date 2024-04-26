using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class AbilitySkill : ObservableObject
	{
		protected string name = string.Empty;
		protected string desc = string.Empty;
		protected string abilityName = string.Empty;
		protected int score;
		protected int miscBonus;
		protected bool skillProficiency;
		protected bool doubleSkillProficiency;
		protected bool halfSkillProficiency;

		public string Name
		{
			get { return name; }
			set { OnPropertyChanged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChanged(ref desc, value); }
		}
		public string AbilityName
		{
			get
			{
				return abilityName;
			}
			set
			{
				OnPropertyChanged(ref abilityName, value);
			}
		}
		public int Score
		{
			get { return score; }
			set { OnPropertyChanged(ref score, value); }
		}
		public int MiscBonus
		{
			get { return miscBonus; }
			set { OnPropertyChanged(ref miscBonus, value); }
		}
		public bool SkillProficiency
		{
			get { return skillProficiency; }
			set { OnPropertyChanged(ref skillProficiency, value); }
		}
		public bool DoubleSkillProficiency
		{
			get { return doubleSkillProficiency; }
			set { OnPropertyChanged(ref doubleSkillProficiency, value); }
		}
		public bool HalfSkillProficiency
		{
			get { return halfSkillProficiency; }
			set { OnPropertyChanged(ref halfSkillProficiency, value); }
		}
	}
}
