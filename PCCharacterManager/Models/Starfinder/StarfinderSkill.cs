﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderSkill : AbilitySkill
	{
		public static StarfinderSkill[] Default =
		{
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
			new StarfinderSkill(),
		};

		private int ranks;
		public int Ranks
		{
			get
			{
				return ranks;
			}
			set
			{
				OnPropertyChanged(ref ranks, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		private int classBonus;
		public int ClassBonus
		{
			get
			{
				return classBonus;
			}
			set
			{
				OnPropertyChanged(ref classBonus, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		public new int Score
		{
			get
			{
				return score;
			}
			set
			{
				OnPropertyChanged(ref score, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		public int Total
		{
			get
			{
				return classBonus + ranks + MiscBonus + Score;
			}
		}

		private bool classSkill;
		public bool ClassSkill
		{
			get
			{
				return classSkill;
			}
			set
			{
				OnPropertyChanged(ref classSkill, value);
			}
		}

		private bool trainedOnly;
		public bool TrainedOnly
		{
			get
			{
				return trainedOnly;
			}
			set
			{
				OnPropertyChanged(ref trainedOnly, value);
			}
		}
	}
}
