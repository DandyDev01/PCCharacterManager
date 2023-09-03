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
		public static StarfinderAbility[] Default = 
		{
			new StarfinderAbility(),
			new StarfinderAbility(),
			new StarfinderAbility(),
			new StarfinderAbility(),
			new StarfinderAbility(),
			new StarfinderAbility()
		};

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
			UpgradedModifier = upgradedScore switch
			{
				1 => -5,
				2 => -4,
				3 => -4,
				4 => -3,
				5 => -3,
				6 => -2,
				7 => -2,
				8 => -1,
				9 => -1,
				10 => 0,
				11 => 0,
				12 => 1,
				13 => 1,
				14 => 2,
				15 => 2,
				16 => 3,
				17 => 3,
				18 => 4,
				19 => 4,
				20 => 5,
				21 => 5,
				22 => 6,
				23 => 6,
				24 => 7,
				25 => 7,
				26 => 8,
				27 => 8,
				28 => 9,
				29 => 9,
				30 => 10,
				_ => -5,
			};
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
			List<StarfinderSkill> skills = new();
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
			if (string.IsNullOrEmpty(skillName)) 
				throw new ArgumentNullException(nameof(skillName), "parameter skillName cannot be null or empty");

			if (string.IsNullOrWhiteSpace(skillName)) 
				throw new ArgumentNullException(nameof(skillName), "parameter skillName cannot be null or whiteSpace");

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
