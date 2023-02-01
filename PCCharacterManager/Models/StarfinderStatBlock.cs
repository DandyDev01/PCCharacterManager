using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderStatBlock : ObservableObject
	{
		public int Total
		{
			get
			{
				return baseValue + abilityMod + miscMod;
			}
		}

		private int baseValue;
		public int BaseValue
		{
			get
			{
				return baseValue;
			}
			set
			{
				OnPropertyChanged(ref baseValue, value);
				OnPropertyChanged("Total");
			}
		}

		private int abilityMod;
		public int AbilityMod
		{
			get
			{
				return abilityMod;
			}
			set
			{
				OnPropertyChanged(ref abilityMod, value);
				OnPropertyChanged("Total");
			}
		}

		private int miscMod;
		public int MiscMod
		{
			get
			{
				return miscMod;
			}
			set
			{
				OnPropertyChanged(ref miscMod, value);
				OnPropertyChanged("Total");
			}
		}

		public StarfinderStatBlock()
		{
			baseValue = 0;
			abilityMod = 0;
			miscMod = 0;
		}
	}
}
