using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderArmorClass : ArmorClass
	{
		public const string energyArmorClassBase = "10";
		public const string kineticArmorClassBase = "10";
		public const string acVsCombatManeuversBase = "8 + KAC";

		private string energyArmorClassValue = string.Empty;
		public string EnergyArmorClassValue
		{
			get
			{
				return energyArmorClassValue;
			}
			set
			{
				OnPropertyChanged(ref energyArmorClassValue, value);
			}
		}

		private string kineticArmorClassValue = string.Empty;
		public string KineticArmorClassValue
		{
			get
			{
				return kineticArmorClassValue;
			}
			set
			{
				OnPropertyChanged(ref kineticArmorClassValue, value);
			}
		}

		private string energyArmorBonus = string.Empty;	
		public string EnergyArmorBonus
		{
			get
			{
				return energyArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref energyArmorBonus, value);
			}
		}

		private string energyDexMod = string.Empty;
		public string EnergyDexMod
		{
			get
			{
				return energyDexMod = string.Empty;
			}
			set
			{
				OnPropertyChanged(ref energyDexMod, value);
			}
		}

		private string energyMiscMod = string.Empty;
		public string EnergyMiscMod
		{
			get
			{
				return energyMiscMod;
			}
			set
			{
				OnPropertyChanged(ref energyMiscMod, value);
			}
		}

		private string kineticArmorBonus = string.Empty;
		public string KineticArmorBonus
		{
			get
			{
				return kineticArmorBonus;
			}
			set
			{
				OnPropertyChanged(ref kineticArmorBonus, value);
			}
		}

		private string kineticDexMod = string.Empty;
		public string KineticDexMod
		{
			get
			{
				return kineticDexMod = string.Empty;
			}
			set
			{
				OnPropertyChanged(ref kineticDexMod, value);
			}
		}

		private string kineticMiscMod = string.Empty;
		public string KineticMiscMod
		{
			get
			{
				return kineticMiscMod;
			}
			set
			{
				OnPropertyChanged(ref kineticMiscMod, value);
			}
		}
	}
}
