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
		public StarfinderAugmentation[] Augmentations { get; set; }
		public new StarfinderArmorClass ArmorClass { get; set; }
		public new StarfinderAbility[] Abilities
		{
			get { return abilities; }
			set { abilities = value; }
		}

		public string HomeWorld { get; set; }
		public string KeyAbilityScore { get; set; }

		public StarfinderCharacter() : base()
		{
			HomeWorld = string.Empty;
			KeyAbilityScore = string.Empty;
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
			Augmentations = Array.Empty<StarfinderAugmentation>();
			Theme = new StarfinderTheme();
			abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, 
			DnD5eBackgroundData backgroundData) : base(classData, raceData, backgroundData)
		{
			HomeWorld = string.Empty;
			KeyAbilityScore = string.Empty;
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
			Augmentations = Array.Empty<StarfinderAugmentation>();
			Theme = new StarfinderTheme();
			abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, StarfinderThemeData themeData)
		{
			HomeWorld = string.Empty;
			KeyAbilityScore = string.Empty;
			CharacterClass = new DnD5eCharacterClass(classData);
			Race = new DnD5eCharacterRace(raceData);
			Theme = new StarfinderTheme(themeData);

			CharacterType = CharacterType.starfinder;
			StaminaPoints = new Property();
			ResolvePoints = new Property();
			ArmorClass = new StarfinderArmorClass();
			MeleeAttack = new StarfinderStatBlock();
			RangedAttack = new StarfinderStatBlock();
			ThrownAttack = new StarfinderStatBlock();
			SavingThrowWill = new StarfinderStatBlock();
			SavingThrowReflex = new StarfinderStatBlock();
			Augmentations = Array.Empty<StarfinderAugmentation>();
			SavingThrowFortitude = new StarfinderStatBlock();
			abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();


			Languages = new ObservableCollection<string>(raceData.Languages);
			HomeWorld = raceData.HomeWorld;
		}
	}
}
