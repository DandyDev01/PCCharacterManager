using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterLevel : ObservableObject, ILevel
	{
		private int level = 1;
		private int experiencePoints;
		private int proficiencyBonus;


		public int Level
		{
			get { return level; }
			set
			{
				if (value < 1)
					value = 1;

				level = value;
				SetProfBonus(this);
				OnPropertyChaged(ref level, value);
			}
		}
		public int ExperiencePoints
		{
			get { return experiencePoints; }
			set
			{
				OnPropertyChaged(ref experiencePoints, value);
			}
		}
		public int ProficiencyBonus
		{
			get { return proficiencyBonus; }
			set
			{
				OnPropertyChaged(ref proficiencyBonus, value);
			}
		}

		private static void SetProfBonus(CharacterLevel level)
		{
			if (level.Level >= 1 && level.Level <= 4)
			{
				level.ProficiencyBonus = 2;
			}
			else if (level.Level >= 5 && level.Level <= 8)
			{
				level.ProficiencyBonus = 3;
			}
			else if (level.Level >= 9 && level.Level <= 12)
			{
				level.ProficiencyBonus = 4;
			}
			else if (level.Level >= 13 && level.Level <= 16)
			{
				level.ProficiencyBonus = 5;
			}
			else
				level.ProficiencyBonus = 6;
		}

		public void LevelUp()
		{
			Level++;
		}
	}
}
