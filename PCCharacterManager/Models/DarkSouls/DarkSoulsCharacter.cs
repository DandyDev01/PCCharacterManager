using PCCharacterManager.Models.DarkSouls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DarkSoulsCharacter : DnD5eCharacter
	{

		private DarkSoulsOrigin _origin;
		public DarkSoulsOrigin Origin { get => _origin; set => _origin = value; }

		public DarkSoulsCharacter(DnD5eCharacterClassData classData, DarkSoulsOrigin oragin)
		{
			CharacterClass = new DnD5eCharacterClass(classData);
			
			_origin = oragin;
			_background = oragin.Name;

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
			_background = _origin.Name;
			Size = CreatureSize.MEDIUM;
			Alignment = Alignment.LAWFUL_GOOD;
			CharacterType = CharacterType.dark_souls;
			_status = CharacterStatus.IDLE;

			NoteManager.NewNoteSection(new NoteSection("Character"));

			AddMovementType(new Property(MovementType.WALK.ToString(), "30ft"));
			AddLanguage("Common");
		}

		public DarkSoulsCharacter() : base()
		{

		}
	}
}
