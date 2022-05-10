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
		private string name = string.Empty;
		private string desc = string.Empty;
		private int score;
		private int miscBonus;
		private bool skillProficiency;
		private bool doubleSkillProficiency;
		private bool halfSkillProficiency;

		public string Name
		{
			get { return name; }
			set { OnPropertyChaged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChaged(ref desc, value); }
		}
		public int Score
		{
			get { return score; }
			set { OnPropertyChaged(ref score, value); }
		}
		public int MiscBonus
		{
			get { return miscBonus; }
			set { OnPropertyChaged(ref miscBonus, value); }
		}
		public bool SkillProficiency
		{
			get { return skillProficiency; }
			set { OnPropertyChaged(ref skillProficiency, value); }
		}
		public bool DoubleSkillProficiency
		{
			get { return doubleSkillProficiency; }
			set { OnPropertyChaged(ref doubleSkillProficiency, value); }
		}
		public bool HalfSkillProficiency
		{
			get { return halfSkillProficiency; }
			set { OnPropertyChaged(ref halfSkillProficiency, value); }
		}
	}
}
