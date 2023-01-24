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
		private int score;
		private int modifier;
		private int save;
		private bool profSave;
		[JsonProperty]
		private int profBonus = 2;

		[JsonProperty]
		public string Name { get; private set; }
		[JsonProperty]
		public string Description { get; private set; }
		public int Score
		{
			get { return score; }
			set
			{
				if (value < 1)
					value = 1;

				OnPropertyChanged(ref score, value);
				SetMod();
				UpdateSkillInfo(profBonus);
				SetSave(profBonus);
				if (profSave)
					Save = modifier + profBonus;
				else
					Save = modifier;
			}
		}
		[JsonProperty]
		public int Modifier
		{
			get { return modifier; }
			private set
			{
				OnPropertyChanged(ref modifier, value);
			}
		}
		[JsonProperty]
		public int Save
		{
			get { return save; }
			private set
			{
				OnPropertyChanged(ref save, value);
			}
		}
		public bool ProfSave
		{
			get { return profSave; }
			set
			{

				OnPropertyChanged(ref profSave, value);
				if (profSave)
					Save = modifier + profBonus;
				else
					Save = modifier;
			}
		}

		[JsonProperty]
		public AbilitySkill[] Skills { get; private set; }

		public ICommand UpdateProfSaveCommand { get; private set; }

		public Ability()
		{
			Name = string.Empty;
			Description = string.Empty;
			Score = 1;
			SetProfBonus(2);
			Skills = Array.Empty<AbilitySkill>();
			UpdateProfSaveCommand = new RelayCommand(SetProfSave);
		}

		public void SetProfBonus(int _profBonus)
		{
			if (_profBonus < 2)
				_profBonus = 2;

			profBonus = _profBonus;
			UpdateSkillInfo(profBonus);
			SetSave(profBonus);
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
			switch (score)
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
		private void UpdateSkillInfo(int profBonus)
		{
			if (Skills == null)
				return;

			foreach (var skill in Skills)
			{

				if (skill.SkillProficiency)
				{
					skill.Score = profBonus + modifier;
					continue;
				}

				skill.Score = modifier;
			}
		}

		private void SetProfSave()
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
		/// <returns></returns>
		public static AbilitySkill FindSkill(string skillName, Ability[] abilities)
		{

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
		/// looks for a skill in a collection of abilities
		/// </summary>
		/// <param name="abilities">collection to search</param>
		/// <param name="skillName">skill you are looking for</param>
		/// <returns>null if no skill exists or skill with specified name</returns>
		public static AbilitySkill FindSkill(Ability[] abilities, string skillName)
		{

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

	} // end class
}
