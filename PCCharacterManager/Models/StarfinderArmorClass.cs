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
				return energyArmorBonus + energyArmorClassBase + energyDexMod + energyMiscMod;
			}
		}
		public int KineticArmorTotal
		{
			get
			{
				return kineticDexMod + kineticMiscMod + kineticArmorClassBase + kineticArmorBonus;
			}
		}
		public int AcVsCombatManeuvers
		{
			get { return acVsCombatManeuversBase + KineticArmorTotal; }
		}

		private int energyArmorBonus;	
		public int EnergyArmorBonus
		{
			get
			{
				return energyArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref energyArmorBonus, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int energyDexMod;
		public int EnergyDexMod
		{
			get
			{
				return energyDexMod;
			}
			set
			{
				OnPropertyChanged(ref energyDexMod, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int energyMiscMod;
		public int EnergyMiscMod
		{
			get
			{
				return energyMiscMod;
			}
			set
			{
				OnPropertyChanged(ref energyMiscMod, value);
				OnPropertyChanged("energyArmorTotal");
			}
		}

		private int kineticArmorBonus;
		public int KineticArmorBonus
		{
			get
			{
				return kineticArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref kineticArmorBonus, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}

		private int kineticDexMod;
		public int KineticDexMod
		{
			get
			{
				return kineticDexMod;
			}
			set
			{
				OnPropertyChanged(ref kineticDexMod, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}

		private int kineticMiscMod;
		public int KineticMiscMod
		{
			get
			{
				return kineticMiscMod;
			}
			set
			{
				OnPropertyChanged(ref kineticMiscMod, value);
				OnPropertyChanged("KineticArmorTotal");
				OnPropertyChanged("AcVsCombatManeuvers");
			}
		}
	}
}
