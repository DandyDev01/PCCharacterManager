using Newtonsoft.Json;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Models
{
	public class StarfinderAbility : Ability
	{
		[JsonProperty]
		public new StarfinderSkill[] Skills { get; private set; }

		private int upgradedScore;
		public int UpgradedScore
		{
			get
			{
				return upgradedScore;
			}
			set
			{
				OnPropertyChanged(ref upgradedScore, value);
				SetUpgradedMod();
				UpdateSkillInfo(0);
			}
		}

		private int upgradedModifier;
		public int UpgradedModifier
		{
			get
			{
				return upgradedModifier;
			}
			set
			{
				OnPropertyChanged(ref upgradedModifier, value);
			}
		}

		public StarfinderAbility()
		{
			Name = string.Empty;
			Description = string.Empty;
			Score = 1;
			SetProfBonus(2);
			UpdateProfSaveCommand = new RelayCommand(SetProfSave);
			Skills = Array.Empty<StarfinderSkill>();
		}

		private void SetUpgradedMod()
		{
			switch (upgradedScore)
			{
				case 1:
					UpgradedModifier = -5;
					break;
				case 2:
					UpgradedModifier = -4;
					break;
				case 3:
					UpgradedModifier = -4;
					break;
				case 4:
					UpgradedModifier = -3;
					break;
				case 5:
					UpgradedModifier = -3;
					break;
				case 6:
					UpgradedModifier = -2;
					break;
				case 7:
					UpgradedModifier = -2;
					break;
				case 8:
					UpgradedModifier = -1;
					break;
				case 9:
					UpgradedModifier = -1;
					break;
				case 10:
					UpgradedModifier = 0;
					break;
				case 11:
					UpgradedModifier = 0;
					break;
				case 12:
					UpgradedModifier = 1;
					break;
				case 13:
					UpgradedModifier = 1;
					break;
				case 14:
					UpgradedModifier = 2;
					break;
				case 15:
					UpgradedModifier = 2;
					break;
				case 16:
					UpgradedModifier = 3;
					break;
				case 17:
					UpgradedModifier = 3;
					break;
				case 18:
					UpgradedModifier = 4;
					break;
				case 19:
					UpgradedModifier = 4;
					break;
				case 20:
					UpgradedModifier = 5;
					break;
				case 21:
					UpgradedModifier = 5;
					break;
				case 22:
					UpgradedModifier = 6;
					break;
				case 23:
					UpgradedModifier = 6;
					break;
				case 24:
					UpgradedModifier = 7;
					break;
				case 25:
					UpgradedModifier = 7;
					break;
				case 26:
					UpgradedModifier = 8;
					break;
				case 27:
					UpgradedModifier = 8;
					break;
				case 28:
					UpgradedModifier = 9;
					break;
				case 29:
					UpgradedModifier = 9;
					break;
				case 30:
					UpgradedModifier = 10;
					break;
				default:
					UpgradedModifier = -5;
					break;
			}
		}

		protected override void UpdateSkillInfo(int profBonus)
		{
			if (Skills == null)
				return;

			foreach (var skill in Skills)
			{

				if (skill.SkillProficiency)
				{
					skill.Score = profBonus + UpgradedModifier;
					continue;
				}

				skill.Score = UpgradedModifier;
			}
		}

		/// <summary>
		/// gets all skills under abilities
		/// </summary>
		/// <param name="abilities">abilities to search</param>
		/// <returns></returns>
		public static StarfinderSkill[] GetSkills(StarfinderAbility[] abilities)
		{
			List<StarfinderSkill> skills = new List<StarfinderSkill>();
			foreach (var item in abilities)
			{
				skills.AddRange(item.Skills);
			}

			return skills.ToArray();
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
		public static StarfinderSkill FindSkill(StarfinderAbility[] abilities, string skillName)
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

	}
}
