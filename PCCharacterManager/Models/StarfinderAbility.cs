using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderAbility : Ability
	{
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
				SetMod();
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

		private void SetMod()
		{
			switch (upgradedScore)
			{
				case 1:
					UpgradedModifier = -5;
					break;
				case 2:
					UpgradedModifier = -4;
					break;
				case 3:
					UpgradedModifier = -4;
					break;
				case 4:
					UpgradedModifier = -3;
					break;
				case 5:
					UpgradedModifier = -3;
					break;
				case 6:
					UpgradedModifier = -2;
					break;
				case 7:
					UpgradedModifier = -2;
					break;
				case 8:
					UpgradedModifier = -1;
					break;
				case 9:
					UpgradedModifier = -1;
					break;
				case 10:
					UpgradedModifier = 0;
					break;
				case 11:
					UpgradedModifier = 0;
					break;
				case 12:
					UpgradedModifier = 1;
					break;
				case 13:
					UpgradedModifier = 1;
					break;
				case 14:
					UpgradedModifier = 2;
					break;
				case 15:
					UpgradedModifier = 2;
					break;
				case 16:
					UpgradedModifier = 3;
					break;
				case 17:
					UpgradedModifier = 3;
					break;
				case 18:
					UpgradedModifier = 4;
					break;
				case 19:
					UpgradedModifier = 4;
					break;
				case 20:
					UpgradedModifier = 5;
					break;
				case 21:
					UpgradedModifier = 5;
					break;
				case 22:
					UpgradedModifier = 6;
					break;
				case 23:
					UpgradedModifier = 6;
					break;
				case 24:
					UpgradedModifier = 7;
					break;
				case 25:
					UpgradedModifier = 7;
					break;
				case 26:
					UpgradedModifier = 8;
					break;
				case 27:
					UpgradedModifier = 8;
					break;
				case 28:
					UpgradedModifier = 9;
					break;
				case 29:
					UpgradedModifier = 9;
					break;
				case 30:
					UpgradedModifier = 10;
					break;
				default:
					UpgradedModifier = -5;
					break;
			}
		}
	}
}
