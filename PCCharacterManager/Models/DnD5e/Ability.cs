using Newtonsoft.Json;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.Models
{
	public class Ability : ObservableObject
	{
		public static Ability[] Default =
		{
			new Ability(1, 1, 1, false, 1, "", "", new AbilitySkill[]
			{

			}),
		};

		private int _score;
		private int _modifier;
		private int _save;
		private bool _profSave;

		[JsonProperty] private int _profBonus = 2;

		public int Score
		{
			get { return _score; }
			set
			{
				if (value < 1)
					value = 1;

				OnPropertyChanged(ref _score, value);
				SetMod();
				UpdateSkillInfo(_profBonus);
				SetSave(_profBonus);
				if (_profSave)
					Save = _modifier + _profBonus;
				else
					Save = _modifier;
			}
		}
		public bool ProfSave
		{
			get { return _profSave; }
			set
			{

				OnPropertyChanged(ref _profSave, value);
				if (_profSave)
					Save = _modifier + _profBonus;
				else
					Save = _modifier;
			}
		}
		[JsonProperty] public string Name { get; protected set; }
		[JsonProperty] public string Description { get; protected set; }
		[JsonProperty] public int Modifier
		{
			get { return _modifier; }
			private set
			{
				OnPropertyChanged(ref _modifier, value);
			}
		}
		[JsonProperty] public int Save
		{
			get { return _save; }
			private set
			{
				OnPropertyChanged(ref _save, value);
			}
		}
		[JsonProperty] public AbilitySkill[] Skills { get; private set; }

		public ICommand UpdateProfSaveCommand { get; protected set; }

		public Ability()
		{
			Name = string.Empty;
			Description = string.Empty;
			Score = 1;
			SetProfBonus(2);
			Skills = Array.Empty<AbilitySkill>();
			UpdateProfSaveCommand = new RelayCommand(SetProfSave);
		}

		public Ability(int score, int modifier, int save, bool profSave, int profBonus,
			string name, string description, AbilitySkill[] skills)
		{
			Score = score;
			Modifier = modifier;
			Save = save;
			ProfSave = profSave;
			_profBonus = profBonus;
			Score = score;
			ProfSave = profSave;
			Name = name;
			Description = description;
			Modifier = modifier;
			Save = save;
			Skills = skills;
			UpdateProfSaveCommand = new RelayCommand(SetProfSave);
		}

		/// <summary>
		/// set the proficiency bonus and update associated skill info and ability save info
		/// </summary>
		/// <param name="profBonus">value to set profBonus too</param>
		/// <exception cref="Exception">when _profBonus is below 0</exception>
		public void SetProfBonus(int profBonus)
		{
			if (profBonus <= 0) 
				throw new Exception("param _profBonus must be greater than 0");

			if (profBonus < 2)
				profBonus = 2;

			this._profBonus = profBonus;
			UpdateSkillInfo(this._profBonus);
			SetSave(this._profBonus);
		}

		/// <summary>
		/// sets the saveing throw for the ability
		/// </summary>
		/// <param name="profBonus">proficiency bonus from character level</param>
		private void SetSave(int profBonus)
		{

		}

		/// <summary>
		/// set the modifier for the ability
		/// </summary>
		private void SetMod()
		{
			switch (_score)
			{
				case 1:
					Modifier = -5;
					break;
				case 2:
					Modifier = -4;
					break;
				case 3:
					Modifier = -4;
					break;
				case 4:
					Modifier = -3;
					break;
				case 5:
					Modifier = -3;
					break;
				case 6:
					Modifier = -2;
					break;
				case 7:
					Modifier = -2;
					break;
				case 8:
					Modifier = -1;
					break;
				case 9:
					Modifier = -1;
					break;
				case 10:
					Modifier = 0;
					break;
				case 11:
					Modifier = 0;
					break;
				case 12:
					Modifier = 1;
					break;
				case 13:
					Modifier = 1;
					break;
				case 14:
					Modifier = 2;
					break;
				case 15:
					Modifier = 2;
					break;
				case 16:
					Modifier = 3;
					break;
				case 17:
					Modifier = 3;
					break;
				case 18:
					Modifier = 4;
					break;
				case 19:
					Modifier = 4;
					break;
				case 20:
					Modifier = 5;
					break;
				case 21:
					Modifier = 5;
					break;
				case 22:
					Modifier = 6;
					break;
				case 23:
					Modifier = 6;
					break;
				case 24:
					Modifier = 7;
					break;
				case 25:
					Modifier = 7;
					break;
				case 26:
					Modifier = 8;
					break;
				case 27:
					Modifier = 8;
					break;
				case 28:
					Modifier = 9;
					break;
				case 29:
					Modifier = 9;
					break;
				case 30:
					Modifier = 10;
					break;
				default:
					Modifier = -5;
					break;

			}

		}

		/// <summary>
		/// update info for the skills
		/// </summary>
		/// <param name="profBonus">proficiency bonus from character level</param>
		protected virtual void UpdateSkillInfo(int profBonus)
		{
			if (Skills == null)
				return;

			foreach (var skill in Skills)
			{
				skill.AbilityModifier = _modifier;
				skill.ProficiencyModifier = profBonus;
				if (skill.SkillProficiency)
				{
					skill.Score = profBonus + _modifier;
					continue;
				}

				skill.Score = _modifier;
			}
		}

		protected void SetProfSave()
		{
			ProfSave = !ProfSave;
		}

		/// <summary>
		/// find ability by name
		/// </summary>
		/// <param name="abilities">abilities to search</param>
		/// <param name="name">name of wanted ability</param>
		/// <returns></returns>
		public static Ability FindAbility(Ability[] abilities, string name)
		{
			foreach (var item in abilities)
			{
				if (item.Name.ToLower().Equals(name.ToLower()))
					return item;
			}
			throw new Exception("Could not find Ability " + name);
		}

		/// <summary>
		/// find ability by skill
		/// </summary>
		/// <param name="abilities">abilities to search</param>
		/// <param name="skill">skill that belongs to the wanted ability</param>
		/// <returns></returns>
		public static Ability FindAbility(Ability[] abilities, AbilitySkill skill)
		{

			foreach (var ability in abilities)
			{
				foreach (var _skill in ability.Skills)
				{
					if (_skill.Name.ToLower().Equals(skill.Name.ToLower()))
						return ability;

				}
			}

			throw new Exception("Could not find Ability with skill " + skill.Name);
		}

		/// <summary>
		/// find a skill by name
		/// </summary>
		/// <param name="skillName">skill wanted</param>
		/// <param name="abilities">abilities to search</param>
		/// <returns>ability that has the skill skillName</returns>
		/// <exception cref="ArgumentNullException">when skillName is null or empty</exception>
		/// <exception cref="Exception">when skillName is null or whitespace</exception>
		/// <exception cref="Exception">when there is no ability with the skill skillName</exception>
		public static AbilitySkill FindSkill(Ability[] abilities, string skillName)
		{
			if (string.IsNullOrEmpty(skillName)) throw new ArgumentNullException("param skillName cannot be null or empty");
			if (string.IsNullOrWhiteSpace(skillName)) throw new Exception("param skillName cannot be null or whiteSpace");

			foreach (var ability in abilities)
			{
				foreach (var skill in ability.Skills)
				{
					if (skill.Name.ToLower().Equals(skillName.ToLower()))
						return skill;
				}
			}

			throw new Exception("no skill with name " + skillName + " exists");
		}

		/// <summary>
		/// gets all skills under abilities
		/// </summary>
		/// <param name="abilities">abilities to search</param>
		/// <returns></returns>
		public static AbilitySkill[] GetSkills(Ability[] abilities)
		{
			List<AbilitySkill> skills = new List<AbilitySkill>();
			foreach (var item in abilities)
			{
				skills.AddRange(item.Skills);
			}

			return skills.ToArray();
		}

		/// <summary>
		/// returns all the abilities names
		/// </summary>
		/// <param name="abilities">abilities to get the names of</param>
		/// <returns></returns>
		public static List<string> GetAbilityNames(Ability[] abilities)
		{
			List<string> names = new List<string>();

			foreach (var ability in abilities)
			{
				names.Add(ability.Name);
			}// end for

			return names;
		}

		public static List<string> GetSkillNames()
		{
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson);
			List<string> skillNames = new List<string>();
			foreach (var ability in abilities)
			{
				foreach (var skill in ability.Skills)
				{
					skillNames.Add(skill.Name);
				}
			}

			return skillNames;
		}

		public static string[] GetProficientSkillNames(Ability[] abilities)
		{
			List<string> results = new();

			foreach (Ability ability in abilities)
			{
				foreach (AbilitySkill skill in ability.Skills)
				{
					if (skill.SkillProficiency)
						results.Add(skill.Name);
				}
			}

			return results.ToArray();
		}

	} // end class
}
