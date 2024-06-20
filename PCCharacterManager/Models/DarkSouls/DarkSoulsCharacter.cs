using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using PCCharacterManager.Models.DarkSouls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DarkSoulsCharacter : CharacterBase
	{
		private Ability[] _abilities;
		public Ability[] Abilities
		{
			get { return _abilities; }
			set { _abilities = value; }
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

		private int _drivePoints;
		public int DrivePoints
		{
			get
			{
				return _drivePoints;
			}
			set
			{
				OnPropertyChanged(ref _drivePoints, value);
			}
		}

		private DarkSoulsOrigin _origin;
		public DarkSoulsOrigin Origin { get => _origin; set => OnPropertyChanged(ref _origin, value); }
		
		public ObservableCollection<Condition> Conditions { get; protected set; }
		public ObservableCollection<Property> MovementTypes_Speeds { get; protected set; }
		public ObservableCollection<string> CombatActions { get; protected set; }
		public ObservableCollection<string> WeaponProficiencies { get; protected set; }
		public ObservableCollection<string> ArmorProficiencies { get; protected set; }
		public ObservableCollection<string> OtherProficiences { get; protected set; }
		
		private CharacterStatus _status;
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

		[JsonProperty(nameof(Size))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }

		public DarkSoulsCharacter(DnD5eCharacterClassData classData, DarkSoulsOrigin oragin)
		{
			CharacterClass = new DnD5eCharacterClass(classData);
			
			_origin = oragin;
			_drivePoints = 0;

			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

			Conditions = new ObservableCollection<Condition>();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>(classData.WeaponProficiencies);
			ArmorProficiencies = new ObservableCollection<string>(classData.ArmorProficiencies);
			CombatActions = new ObservableCollection<string>();
			OtherProficiences = new ObservableCollection<string>();

			CharacterClass = new DnD5eCharacterClass(classData);
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

			_id = string.Empty;
			_name = string.Empty;
			_dateModified = string.Empty;
			Size = CreatureSize.MEDIUM;
			CharacterType = CharacterType.dark_souls;
			_status = CharacterStatus.IDLE;
			_initiative = Abilities.First(x => x.Name == "Dexterity").Modifier;

			NoteManager.NewNoteSection(new NoteSection("Character"));

			MovementTypes_Speeds.Add(new Property(MovementType.WALK.ToString(), "30ft"));
		}

		public DarkSoulsCharacter() : base()
		{
			Conditions = new ObservableCollection<Condition>();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>();
			ArmorProficiencies = new ObservableCollection<string>();
			CombatActions = new ObservableCollection<string>();
			OtherProficiences = new ObservableCollection<string>();

			_origin = new DarkSoulsOrigin();

			_drivePoints = 0;

			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
		}
	}
}
