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
		protected string _name = string.Empty;
		protected string _desc = string.Empty;
		protected string _abilityName = string.Empty;
		protected int _score;
		protected int _abilityModifier;
		protected int _proficiencyModifier;
		protected int _miscBonus;
		protected bool _skillProficiency;

		public string Name
		{
			get { return _name; }
			set { OnPropertyChanged(ref _name, value); }
		}
		public string Desc
		{
			get { return _desc; }
			set { OnPropertyChanged(ref _desc, value); }
		}
		public string AbilityName
		{
			get
			{
				return _abilityName;
			}
			set
			{
				OnPropertyChanged(ref _abilityName, value);
			}
		}
		public int Score
		{
			get { return _score; }
			set { OnPropertyChanged(ref _score, value); }
		}
		public int AbilityModifier
		{
			get { return _abilityModifier; }
			set { OnPropertyChanged(ref _abilityModifier, value); }
		}
		public int ProficiencyModifier
		{
			get { return _proficiencyModifier; }
			set { OnPropertyChanged(ref _proficiencyModifier, value); }
		}
		public int MiscBonus
		{
			get { return _miscBonus; }
			set { OnPropertyChanged(ref _miscBonus, value); }
		}
		public bool SkillProficiency
		{
			get { return _skillProficiency; }
			set 
			{ 
				OnPropertyChanged(ref _skillProficiency, value);

				int score = _skillProficiency ? _abilityModifier + _proficiencyModifier : _abilityModifier;
				Score = score;
			}
		}
	}
}
