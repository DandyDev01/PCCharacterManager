﻿using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PCCharacterManager.Models
{
	public class StarfinderCharacter : CharacterBase
	{
		private int _initiative;
		public int Initiative
		{
			get { return _initiative; }
			set
			{
				OnPropertyChanged(ref _initiative, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}

		private StarfinderAbility[] _abilities;
		public StarfinderAbility[] Abilities
		{
			get { return _abilities; }
			set { _abilities = value; }
		}

		public ObservableCollection<Property> MovementTypes_Speeds { get; protected set; }
		public ObservableCollection<string> WeaponProficiencies { get; protected set; }
		public ObservableCollection<string> ArmorProficiencies { get; protected set; }
		public ObservableCollection<string> OtherProficiences { get; protected set; }
		public ObservableCollection<string> ToolProficiences { get; protected set; }
		public ObservableCollection<string> Languages { get; protected set; }

		public Property StaminaPoints { get; set; }
		public Property ResolvePoints { get; set; }
		public StarfinderTheme Theme { get; set; }
		public StarfinderStatBlock MeleeAttack { get; set; }
		public StarfinderStatBlock RangedAttack { get; set; }
		public StarfinderStatBlock ThrownAttack { get; set; }
		public StarfinderStatBlock SavingThrowWill { get; set; }
		public StarfinderStatBlock SavingThrowReflex { get; set; }
		public StarfinderStatBlock SavingThrowFortitude { get; set; }
		public ObservableCollection<StarfinderAugmentation> Augmentations { get; set; }
		public new StarfinderArmorClass ArmorClass { get; set; }

		public string HomeWorld { get; set; }
		public string KeyAbilityScore { get; set; }
		public override int CarryWeight => Abilities.Where(x => x.Name == "Strength").First().Score / 2;

		[JsonProperty(nameof(Size))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }

		[JsonProperty(nameof(Alignment))]
		[JsonConverter(typeof(StringEnumConverter))]
		public Alignment Alignment { get; set; }


		public StarfinderCharacter() : base()
		{
			if (Directory.Exists(StarfinderResources.Root) == false)
				return;

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
			Augmentations = new ObservableCollection<StarfinderAugmentation>();
			Theme = new StarfinderTheme();
			_abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, 
			DnD5eBackgroundData backgroundData) : base(classData, raceData, backgroundData)
		{
			if (Directory.Exists(StarfinderResources.Root) == false)
				return;

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
			Augmentations = new ObservableCollection<StarfinderAugmentation>();
			Theme = new StarfinderTheme();
			_abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();
		}

		public StarfinderCharacter(StarfinderClassData classData, StarfinderRaceData raceData, StarfinderThemeData themeData)
		{
			if (Directory.Exists(StarfinderResources.Root) == false)
				return;

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
			Augmentations = new ObservableCollection<StarfinderAugmentation>();
			SavingThrowFortitude = new StarfinderStatBlock();
			_abilities = ReadWriteJsonCollection<StarfinderAbility>.ReadCollection(StarfinderResources.AbilitiesJson).ToArray();


			Languages = new ObservableCollection<string>(raceData.Languages);
			HomeWorld = raceData.HomeWorld;
		}
	}
}
