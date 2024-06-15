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
	public enum CharacterType { DnD5e, starfinder, dark_souls }
	public enum CharacterStatus { COMBAT, IDLE }

	public class DnD5eCharacter : ObservableObject
	{
		public static DnD5eCharacter Default => new();

		protected Ability[] _abilities;

		protected string _name;
		public string Name
		{
			get { return _name; }
			set 
			{ 
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _name, value);
			}
		}

		protected string _id;
		public string Id
		{
			get => _id;
			set => OnPropertyChanged(ref _id, value);
		}

		protected string _background;
		public string Background
		{
			get { return _background; }
			set 
			{ 
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _background, value);
			}
		}
		
		protected string _dateModified;
		public string DateModified
		{
			get { return _dateModified; }	
			set 
			{ 
				OnPropertyChanged(ref _dateModified, value);
			}
		}
		
		protected int _initiative;
		public int Initiative
		{
			get { return _initiative; }
			set 
			{ 
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _initiative, value); 
			}
		}
		
		protected int _passivePerception;
		public int PassivePerception
		{
			get { return _passivePerception; }
			set 
			{
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _passivePerception, value);
			}
		}
		
		protected int _passiveInsight;
		public int PassiveInsight
		{
			get { return _passiveInsight; }
			set 
			{ 
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _passiveInsight, value);
			}
		}

		private int _spentHitDie;
		public int SpentHitDie
		{
			get
			{
				return _spentHitDie;
			}
			set
			{
				OnCharacterChangedAction?.Invoke(this);
				value = Math.Min(value, Level.Level);
				OnPropertyChanged(ref _spentHitDie, value);
			}
		}

		protected int _combatRound;
		public int CombatRound
		{
			get
			{
				return _combatRound;
			}
			set
			{
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _combatRound, value);
			}
		}

		protected bool _isInCombat;
		public bool IsInCombat
		{
			get
			{
				return _isInCombat;
			}
			set
			{
				OnCharacterChangedAction?.Invoke(this);
				OnPropertyChanged(ref _isInCombat, value);
			}
		}

		protected CharacterStatus _status;
		[JsonProperty(nameof(_status))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CharacterStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				OnPropertyChanged(ref _status, value);
			}
		}

		public virtual int CarryWeight => Abilities.Where(x => x.Name == "Strength").First().Score * 15;

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
			get { return _abilities; }
			set { _abilities = value; }
		}

		public ObservableCollection<Condition> Conditions { get; protected set; }
		public ObservableCollection<Property> MovementTypes_Speeds { get; protected set; }
		public ObservableCollection<string> CombatActions { get; set; }
		public ObservableCollection<string> WeaponProficiencies { get;protected set; }
		public ObservableCollection<string> ArmorProficiencies { get;protected set; }
		public ObservableCollection<string> OtherProficiences { get; protected set; }
		public ObservableCollection<string> ToolProficiences { get; protected set; }
		public ObservableCollection<string> Languages { get; protected set; }

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
		public Action<DnD5eCharacter>? OnCharacterChangedAction { get; set; }

		public DnD5eCharacter()
		{
			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

			_status = CharacterStatus.IDLE;

			Conditions = new ObservableCollection<Condition>();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>();
			CombatActions = new ObservableCollection<string>();
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

			Conditions.CollectionChanged += OnCharacterChanged;
			MovementTypes_Speeds.CollectionChanged += OnCharacterChanged;
			WeaponProficiencies.CollectionChanged += OnCharacterChanged;
			ArmorProficiencies.CollectionChanged += OnCharacterChanged;
			OtherProficiences.CollectionChanged += OnCharacterChanged;
			ToolProficiences.CollectionChanged += OnCharacterChanged;
			Languages.CollectionChanged += OnCharacterChanged;

			_id = string.Empty;
			_name = string.Empty;
			_dateModified = string.Empty;
			_background = string.Empty;
			CharacterType = CharacterType.DnD5e;
		}

		public DnD5eCharacter(DnD5eCharacterClassData classData, DnD5eCharacterRaceData raceData, 
			DnD5eBackgroundData backgroundData)
		{
			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

			Conditions = new ObservableCollection<Condition>();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>(classData.WeaponProficiencies);
			ArmorProficiencies = new ObservableCollection<string>(classData.ArmorProficiencies);
			CombatActions = new ObservableCollection<string>();
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

			Conditions.CollectionChanged += OnCharacterChanged;
			WeaponProficiencies.CollectionChanged += OnCharacterChanged;
			ArmorProficiencies.CollectionChanged += OnCharacterChanged;
			OtherProficiences.CollectionChanged += OnCharacterChanged;
			ToolProficiences.CollectionChanged += OnCharacterChanged;

			_id = string.Empty;
			_name = string.Empty;
			_dateModified = string.Empty;
			_background = backgroundData.Name;
			Size = raceData.Size;
			Alignment = Alignment.LAWFUL_GOOD;
			CharacterType = CharacterType.DnD5e;
			_status = CharacterStatus.IDLE;

			NoteManager.NewNoteSection(new NoteSection("Character"));
			NoteManager.GetSection("Character")!.Add(new Note(backgroundData.Name, backgroundData.Desc));

			AddMovementType(new Property(MovementType.WALK.ToString(), raceData.Speed));
			AddLanguages(raceData.Languages);
		}

		/// <summary>
		/// Used to notify when any aspect of the character changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCharacterChanged(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			OnCharacterChangedAction?.Invoke(this);
		}

		/// <summary>
		/// adds a new way to move to the monster
		/// </summary>
		/// <param name="movementTypeToAdd">the movement type to add</param>
		public bool AddMovementType(Property movementTypeToAdd)
		{
			if (MovementTypes_Speeds.Contains(movementTypeToAdd))
				return false;

			OnCharacterChangedAction?.Invoke(this);

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

			OnCharacterChangedAction?.Invoke(this);

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
