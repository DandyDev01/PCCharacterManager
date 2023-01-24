using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderCharacter : Character
	{
		public Property StaminaPoins { get; set; }
		public Property ResolvePoints { get; set; }

		public AttackBonus MeleeAttack { get; set; }
		public AttackBonus RangedAttack { get; set; }
		public AttackBonus ThrownAttack { get; set; }

		public StarfinderCharacter() : base()
		{
			CharacterType = CharacterType.starfinder;
			ArmorClass = new StarfinderArmorClass();
			StaminaPoins = new Property();
			ResolvePoints = new Property();
			MeleeAttack = new AttackBonus();
			RangedAttack = new AttackBonus();
			ThrownAttack = new AttackBonus();
		}

		public StarfinderCharacter(CharacterClassData classData, CharacterRaceData raceData, 
			BackgroundData backgroundData) : base(classData, raceData, backgroundData)
		{
			CharacterType = CharacterType.starfinder;
			ArmorClass = new StarfinderArmorClass();
			StaminaPoins = new Property();
			ResolvePoints = new Property();
			MeleeAttack = new AttackBonus();
			RangedAttack = new AttackBonus();
			ThrownAttack = new AttackBonus();
		}
	}
}
