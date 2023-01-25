using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderCharacter : DnD5eCharacter
	{
		public Property StaminaPoints { get; set; }
		public Property ResolvePoints { get; set; }
		public StarfinderTheme Theme { get; set; }
		public StarfinderAttackBonus MeleeAttack { get; set; }
		public StarfinderAttackBonus RangedAttack { get; set; }
		public StarfinderAttackBonus ThrownAttack { get; set; }
		public new StarfinderArmorClass ArmorClass { get; set; }

		public StarfinderCharacter() : base()
		{
			CharacterType = CharacterType.starfinder;
			ArmorClass = new StarfinderArmorClass();
			StaminaPoints = new Property();
			ResolvePoints = new Property();
			MeleeAttack = new StarfinderAttackBonus();
			RangedAttack = new StarfinderAttackBonus();
			ThrownAttack = new StarfinderAttackBonus();
			Theme = new StarfinderTheme();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, 
			DnD5eBackgroundData backgroundData) : base(classData, raceData, backgroundData)
		{
			CharacterType = CharacterType.starfinder;
			ArmorClass = new StarfinderArmorClass();
			StaminaPoints = new Property();
			ResolvePoints = new Property();
			MeleeAttack = new StarfinderAttackBonus();
			RangedAttack = new StarfinderAttackBonus();
			ThrownAttack = new StarfinderAttackBonus();
			Theme = new StarfinderTheme();
		}
	}
}
