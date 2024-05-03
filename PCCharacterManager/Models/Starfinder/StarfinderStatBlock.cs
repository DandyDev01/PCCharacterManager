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
				return _baseValue + _abilityMod + _miscMod;
			}
		}

		private int _baseValue;
		public int BaseValue
		{
			get
			{
				return _baseValue;
			}
			set
			{
				OnPropertyChanged(ref _baseValue, value);
				OnPropertyChanged("Total");
			}
		}

		private int _abilityMod;
		public int AbilityMod
		{
			get
			{
				return _abilityMod;
			}
			set
			{
				OnPropertyChanged(ref _abilityMod, value);
				OnPropertyChanged("Total");
			}
		}

		private int _miscMod;
		public int MiscMod
		{
			get
			{
				return _miscMod;
			}
			set
			{
				OnPropertyChanged(ref _miscMod, value);
				OnPropertyChanged("Total");
			}
		}

		public StarfinderStatBlock()
		{
			_baseValue = 0;
			_abilityMod = 0;
			_miscMod = 0;
		}
	}
}
