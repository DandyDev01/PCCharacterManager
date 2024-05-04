using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderArmorClass : ArmorClass
	{
		public const int energyArmorClassBase = 10;
		public const int kineticArmorClassBase = 10;
		public const int acVsCombatManeuversBase = 8;

		public int EnergyArmorTotal
		{
			get
			{
				return _energyArmorBonus + energyArmorClassBase + _energyDexMod + _energyMiscMod;
			}
		}
		public int KineticArmorTotal
		{
			get
			{
				return _kineticDexMod + _kineticMiscMod + kineticArmorClassBase + _kineticArmorBonus;
			}
		}
		public int AcVsCombatManeuvers
		{
			get { return acVsCombatManeuversBase + KineticArmorTotal; }
		}

		private int _energyArmorBonus;	
		public int EnergyArmorBonus
		{
			get
			{
				return _energyArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref _energyArmorBonus, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int _energyDexMod;
		public int EnergyDexMod
		{
			get
			{
				return _energyDexMod;
			}
			set
			{
				OnPropertyChanged(ref _energyDexMod, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int _energyMiscMod;
		public int EnergyMiscMod
		{
			get
			{
				return _energyMiscMod;
			}
			set
			{
				OnPropertyChanged(ref _energyMiscMod, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int _kineticArmorBonus;
		public int KineticArmorBonus
		{
			get
			{
				return _kineticArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref _kineticArmorBonus, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}

		private int _kineticDexMod;
		public int KineticDexMod
		{
			get
			{
				return _kineticDexMod;
			}
			set
			{
				OnPropertyChanged(ref _kineticDexMod, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}

		private int _kineticMiscMod;
		public int KineticMiscMod
		{
			get
			{
				return _kineticMiscMod;
			}
			set
			{
				OnPropertyChanged(ref _kineticMiscMod, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}
	}
}
