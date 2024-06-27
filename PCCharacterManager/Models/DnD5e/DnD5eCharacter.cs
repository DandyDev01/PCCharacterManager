using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
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

	public class DnD5eCharacter : CharacterBase
	{
		private Ability[] _abilities;
		public Ability[] Abilities
		{
			get { return _abilities; }
			set { _abilities = value; }
		}

		private string _background;
		public string Background
		{
			get { return _background; }
			set 
			{ 
				OnPropertyChanged(ref _background, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}
		
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
		
		private int _passivePerception;
		public int PassivePerception
		{
			get { return _passivePerception; }
			set 
			{
				OnPropertyChanged(ref _passivePerception, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}
		
		private int _passiveInsight;
		public int PassiveInsight
		{
			get { return _passiveInsight; }
			set 
			{ 
				OnPropertyChanged(ref _passiveInsight, value);
				OnCharacterChangedAction?.Invoke(this);
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
				value = Math.Min(value, Level.Level);
				OnPropertyChanged(ref _spentHitDie, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}

		private int _combatRound;
		public int CombatRound
		{
			get
			{
				return _combatRound;
			}
			set
			{
				OnPropertyChanged(ref _combatRound, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}

		private bool _isInCombat;
		public bool IsInCombat
		{
			get
			{
				return _isInCombat;
			}
			set
			{
				OnPropertyChanged(ref _isInCombat, value);
				OnCharacterChangedAction?.Invoke(this);
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

		public override int CarryWeight => Abilities.Where(x => x.Name == "Strength").First().Score * 15;

		public ObservableCollection<Condition> Conditions { get; protected set; }
		public ObservableCollection<Property> MovementTypes_Speeds { get; protected set; }
		public ObservableCollection<string> CombatActions { get; protected set; }
		public ObservableCollection<string> WeaponProficiencies { get;protected set; }
		public ObservableCollection<string> ArmorProficiencies { get;protected set; }
		public ObservableCollection<string> OtherProficiences { get; protected set; }
		public ObservableCollection<string> ToolProficiences { get; protected set; }
		public ObservableCollection<string> Languages { get; protected set; }

		[JsonProperty(nameof(Size))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }

		[JsonProperty(nameof(Alignment))]
		[JsonConverter(typeof(StringEnumConverter))]
		public Alignment Alignment { get; set; }

		public DnD5eCharacter()
		{
			if (Directory.Exists(DnD5eResources.Root) == false)
				return;

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

			CharacterClass.PropertyChanged += OnCharacterChanged;
			ArmorClass.PropertyChanged += OnCharacterChanged;
			Level.PropertyChanged += OnCharacterChanged;
			Health.PropertyChanged += OnCharacterChanged;
			SpellBook.PropertyChanged += OnCharacterChanged;
			SpellBook.CantripsKnown.CollectionChanged += OnCharacterChanged;
			SpellBook.PreparedSpells.CollectionChanged += OnCharacterChanged;

			foreach (var item in SpellBook.SpellsKnown)
			{
				item.Value.CollectionChanged += OnCharacterChanged;
			}

			foreach (var item in Inventory.Items)
			{
				item.Value.CollectionChanged += OnCharacterChanged;
			}

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
			if (Directory.Exists(DnD5eResources.Root) == false)
				return;

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

			CharacterClass.PropertyChanged += OnCharacterChanged;
			ArmorClass.PropertyChanged += OnCharacterChanged;
			Level.PropertyChanged += OnCharacterChanged;
			Health.PropertyChanged += OnCharacterChanged;
			SpellBook.PropertyChanged += OnCharacterChanged;
			SpellBook.CantripsKnown.CollectionChanged += OnCharacterChanged;
			SpellBook.PreparedSpells.CollectionChanged += OnCharacterChanged;

			foreach (var item in SpellBook.SpellsKnown)
			{
				item.Value.CollectionChanged += OnCharacterChanged;
			}

			foreach (var item in Inventory.Items)
			{
				item.Value.CollectionChanged += OnCharacterChanged;
			}

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
		/// adds a new way to move to the monster
		/// </summary>
		/// <param name="movementTypeToAdd">the movement type to add</param>
		public bool AddMovementType(Property movementTypeToAdd)
		{
			if (MovementTypes_Speeds.Contains(movementTypeToAdd))
				return false;

			MovementTypes_Speeds.Add(movementTypeToAdd);

			OnCharacterChangedAction?.Invoke(this);
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

			OnCharacterChangedAction?.Invoke(this);
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
