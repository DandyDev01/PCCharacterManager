﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public enum Alignment { LAWFUL_GOOD, NEUTRAL_GOOD, CHAOTIC_GOOD, LAWFUL_NEUTRAL, TRUE_NEUTRAL, CHAOTIC_NEUTRAL, LAWFUL_EVIL, NEUTRAL_EVIL, CHAOTIC_EVIL, UNALIGNED }
	public enum HitDie { D4, D6, D8, D10, D12, D20 }
	public enum MovementType { BURROW, CLIMB, FLY, SWIM, WALK }
	public enum DamageType { SLASHING, PIERCING, BLUDGENING, POISON, ACID, FIRE, COLD, RADIANT, NECROTIC, LIGHTING, THUNDER, FORCE, PSYCHIC };
	public enum CreatureSize { TINY, SMALL, MEDIUM, LARGE, HUGE, GARGANTUAN }
	public enum CharacterType { DnD5e, starfinder }

	public class DnD5eCharacter : ObservableObject
	{
		public static DnD5eCharacter Default => new DnD5eCharacter();

		protected Ability[] abilities;

		protected string name;
		public string Name
		{
			get { return name; }
			set 
			{ 
				OnPropertyChanged(ref name, value); 
			}
		}

		protected string background;
		public string Background
		{
			get { return background; }
			set { OnPropertyChanged(ref background, value); }
		}
		
		protected string dateModified;
		public string DateModified
		{
			get { return dateModified; }	
			set { OnPropertyChanged(ref dateModified, value); }
		}
		
		protected int initiative;
		public int Initiative
		{
			get { return initiative; }
			set { OnPropertyChanged(ref initiative, value); }
		}
		
		protected int passivePerception;
		public int PassivePerception
		{
			get { return passivePerception; }
			set { OnPropertyChanged(ref passivePerception, value); }
		}
		
		protected int passiveInsight;
		public int PassiveInsight
		{
			get { return passiveInsight; }
			set { OnPropertyChanged(ref passiveInsight, value); }
		}

		public DnD5eCharacterClass CharacterClass { get; set; }
		public DnD5eCharacterRace Race { get; set; }
		public ArmorClass ArmorClass { get; set; }
		public CharacterLevel Level { get; set; }
		public NoteBook NoteManager { get; set; }
		public Inventory Inventory { get; set; }
		public SpellBook SpellBook { get; set; }
		public Health Health { get; set; }
		public Ability[] Abilities
		{
			get { return abilities; }
			set { abilities = value; }
		}

		public ObservableCollection<Property> MovementTypes_Speeds { get; set; }
		public ObservableCollection<string> WeaponProficiencies { get; set; }
		public ObservableCollection<string> ArmorProficiencies { get; set; }
		public ObservableCollection<string> OtherProficiences { get; set; }
		public ObservableCollection<string> ToolProficiences { get; set; }
		public ObservableCollection<string> Languages { get; set; }

		[JsonProperty(nameof(Size))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }

		[JsonProperty(nameof(CharacterType))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CharacterType CharacterType { get; set; }

		[JsonProperty(nameof(Alignment))]
		[JsonConverter(typeof(StringEnumConverter))]
		public Alignment Alignment { get; set; }

		[JsonIgnore]
		public Action? OnCharacterChangedAction { get; set; }

		public DnD5eCharacter()
		{
			abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
			
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>();
			ArmorProficiencies = new ObservableCollection<string>();
			OtherProficiences = new ObservableCollection<string>();
			ToolProficiences = new ObservableCollection<string>();
			Languages = new ObservableCollection<string>();

			CharacterClass = new DnD5eCharacterClass();
			Race = new DnD5eCharacterRace();
			ArmorClass = new ArmorClass();
			Level = new CharacterLevel();
			NoteManager = new NoteBook();
			SpellBook = new SpellBook();
			Inventory = new Inventory();
			Health = new Health(1);

			MovementTypes_Speeds.CollectionChanged += OnCharacterChanged;
			WeaponProficiencies.CollectionChanged += OnCharacterChanged;
			ArmorProficiencies.CollectionChanged += OnCharacterChanged;
			OtherProficiences.CollectionChanged += OnCharacterChanged;
			ToolProficiences.CollectionChanged += OnCharacterChanged;
			Languages.CollectionChanged += OnCharacterChanged;

			name = string.Empty;
			dateModified = string.Empty;
			background = string.Empty;
			CharacterType = CharacterType.DnD5e;
		}

		public DnD5eCharacter(DnD5eCharacterClassData classData, DnD5eCharacterRaceData raceData, 
			DnD5eBackgroundData backgroundData)
		{
			abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>(classData.WeaponProficiencies);
			ArmorProficiencies = new ObservableCollection<string>(classData.ArmorProficiencies);
			OtherProficiences = new ObservableCollection<string>();
			ToolProficiences = new ObservableCollection<string>();
			Languages = new ObservableCollection<string>();
			
			CharacterClass = new DnD5eCharacterClass(classData);
			Race = new DnD5eCharacterRace(raceData);
			ArmorClass = new ArmorClass();
			Level = new CharacterLevel();
			NoteManager = new NoteBook();
			SpellBook = new SpellBook();
			Inventory = new Inventory();
			Health = new Health(1);
			
			name = string.Empty;
			dateModified = string.Empty;
			background = backgroundData.Name;
			Size = raceData.Size;
			Alignment = Alignment.LAWFUL_GOOD;
			CharacterType = CharacterType.DnD5e;

			NoteManager.NewNoteSection(new NoteSection("Character"));
			NoteManager.GetSection("Character")!.Add(new Note(backgroundData.Name, backgroundData.Desc));

			AddMovementType(new Property(MovementType.WALK.ToString(), raceData.Speed));
			AddLanguages(raceData.Languages);
		}

		private void OnCharacterChanged(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			OnCharacterChangedAction?.Invoke();
		}

		/// <summary>
		/// adds a new way to move to the monster
		/// </summary>
		/// <param name="movementTypeToAdd">the movement type to add</param>
		public bool AddMovementType(Property movementTypeToAdd)
		{
			if (MovementTypes_Speeds.Contains(movementTypeToAdd))
				return false;

			MovementTypes_Speeds.Add(movementTypeToAdd);
			return true;
		}

		/// <summary>
		/// adds a language that the monster now speaks
		/// </summary>
		/// <param name="language">language to learn</param>
		public void AddLanguage(string language)
		{
			if (Languages.Contains(language))
				return;

			Languages.Add(language);
		}

		/// <summary>
		/// Adds a string[]
		/// </summary>
		/// <param name="languages">array of strings</param>
		public void AddLanguages(string[] languages)
		{
			foreach (var language in languages)
			{
				AddLanguage(language);
			}
		}
	}
}
