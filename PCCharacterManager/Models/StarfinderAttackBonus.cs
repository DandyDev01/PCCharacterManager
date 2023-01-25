using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderAttackBonus : ObservableObject
	{
		public string Total
		{
			get
			{
				return (int.Parse(baseAttackBonus) + int.Parse(skillMod) + int.Parse(miscMod)).ToString();
			}
		}

		private string baseAttackBonus;
		public string BaseAttackBonus
		{
			get
			{
				return baseAttackBonus;
			}
			set
			{
				OnPropertyChanged(ref baseAttackBonus, value);
				OnPropertyChaged("Total");
			}
		}

		private string skillMod;
		public string SkillMod
		{
			get
			{
				return skillMod;
			}
			set
			{
				OnPropertyChanged(ref skillMod, value);
				OnPropertyChaged("Total");
			}
		}

		private string miscMod;
		public string MiscMod
		{
			get
			{
				return miscMod;
			}
			set
			{
				OnPropertyChanged(ref miscMod, value);
				OnPropertyChaged("Total");
			}
		}

		public StarfinderAttackBonus()
		{
			baseAttackBonus = "0";
			skillMod = "0";
			miscMod = "0";
		}
	}
}
