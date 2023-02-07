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
		private new StarfinderAbility[] abilities;

		public Property StaminaPoints { get; set; }
		public Property ResolvePoints { get; set; }
		public StarfinderTheme Theme { get; set; }
		public StarfinderStatBlock MeleeAttack { get; set; }
		public StarfinderStatBlock RangedAttack { get; set; }
		public StarfinderStatBlock ThrownAttack { get; set; }
		public StarfinderStatBlock SavingThrowWill { get; set; }
		public StarfinderStatBlock SavingThrowReflex { get; set; }
		public StarfinderStatBlock SavingThrowFortitude { get; set; }
		public new StarfinderArmorClass ArmorClass { get; set; }
		public new StarfinderAbility[] Abilities
		{
			get { return abilities; }
			set { abilities = value; }
		}

		public string HomeWorld { get; set; }

		public StarfinderCharacter() : base()
		{
			CharacterType = CharacterType.starfinder;
			StaminaPoints = new Property();
			ResolvePoints = new Property();
			ArmorClass = new StarfinderArmorClass();
			MeleeAttack = new StarfinderStatBlock();
			RangedAttack = new StarfinderStatBlock();
			ThrownAttack = new StarfinderStatBlock();
			SavingThrowWill = new StarfinderStatBlock();
			SavingThrowReflex = new StarfinderStatBlock();
			SavingThrowFortitude = new StarfinderStatBlock();
			Theme = new StarfinderTheme();
			abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, 
			DnD5eBackgroundData backgroundData) : base(classData, raceData, backgroundData)
		{
			CharacterType = CharacterType.starfinder;
			StaminaPoints = new Property();
			ResolvePoints = new Property();
			ArmorClass = new StarfinderArmorClass();
			MeleeAttack = new StarfinderStatBlock();
			RangedAttack = new StarfinderStatBlock();
			ThrownAttack = new StarfinderStatBlock();
			SavingThrowWill = new StarfinderStatBlock();
			SavingThrowReflex = new StarfinderStatBlock();
			SavingThrowFortitude = new StarfinderStatBlock();
			Theme = new StarfinderTheme();
			abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}
	}
}
