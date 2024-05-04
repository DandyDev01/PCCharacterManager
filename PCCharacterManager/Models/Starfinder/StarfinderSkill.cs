using System;
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

		private int _ranks;
		public int Ranks
		{
			get
			{
				return _ranks;
			}
			set
			{
				OnPropertyChanged(ref _ranks, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		private int _classBonus;
		public int ClassBonus
		{
			get
			{
				return _classBonus;
			}
			set
			{
				OnPropertyChanged(ref _classBonus, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		public new int Score
		{
			get
			{
				return _score;
			}
			set
			{
				OnPropertyChanged(ref _score, value);
				OnPropertyChanged(nameof(Total));
			}
		}

		public int Total
		{
			get
			{
				return _classBonus + _ranks + MiscBonus + Score;
			}
		}

		private bool _classSkill;
		public bool ClassSkill
		{
			get
			{
				return _classSkill;
			}
			set
			{
				OnPropertyChanged(ref _classSkill, value);
			}
		}

		private bool _trainedOnly;
		public bool TrainedOnly
		{
			get
			{
				return _trainedOnly;
			}
			set
			{
				OnPropertyChanged(ref _trainedOnly, value);
			}
		}
	}
}
