using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		protected string name;
		protected string background;
		protected string dateModified;
		protected int initiative;
		protected int passivePerception;
		protected int passiveInsight;
		protected Ability[] abilities;

		public string Name
		{
			get { return name; }
			set 
			{ 
				OnPropertyChanged(ref name, value); 
			}
		}
		public string Background
		{
			get { return background; }
			set { OnPropertyChanged(ref background, value); }
		}
		public string DateModified
		{
			get { return dateModified; }	
			set { OnPropertyChanged(ref dateModified, value); }
		}
		public int Initiative
		{
			get { return initiative; }
			set { OnPropertyChanged(ref initiative, value); }
		}
		public int PassivePerception
		{
			get { return passivePerception; }
			set { OnPropertyChanged(ref passivePerception, value); }
		}
		public int PassiveInsight
		{
			get { return passiveInsight; }
			set { OnPropertyChanged(ref passiveInsight, value); }
		}

		[JsonProperty("Size")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CreatureSize Size { get; set; }
		[JsonProperty("CharacterType")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CharacterType CharacterType { get; set; }
		[JsonProperty("Alignment")]
		[JsonConverter(typeof(StringEnumConverter))]
		public Alignment Alignment { get; set; }

		public ArmorClass ArmorClass { get; set; }
		public DnD5eCharacterClass CharacterClass { get; set; }
		public DnD5eCharacterRace Race { get; set; }
		public Inventory Inventory { get; set; }
		public SpellBook SpellBook { get; set; }
		public Health Health { get; set; }
		public NoteBook NoteManager { get; set; }
		public Ability[] Abilities
		{
			get { return abilities; }
			set { abilities = value; }
		}
		public CharacterLevel Level { get; set; }

		public ObservableCollection<string> Languages { get; set; }
		public ObservableCollection<string> ToolProficiences { get; set; }
		public ObservableCollection<string> OtherProficiences { get; set; }
		public ObservableCollection<string> WeaponProficiencies { get; set; }
		public ObservableCollection<string> ArmorProficiencies { get; set; }
		public ObservableCollection<Property> MovementTypes_Speeds { get; set; }

		public DnD5eCharacter()
		{
			Inventory = new Inventory();
			ToolProficiences = new ObservableCollection<string>();
			OtherProficiences = new ObservableCollection<string>();
			SpellBook = new SpellBook();
			NoteManager = new NoteBook();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			WeaponProficiencies = new ObservableCollection<string>();
			ArmorProficiencies = new ObservableCollection<string>();
			Languages = new ObservableCollection<string>();
			abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
			Level = new CharacterLevel();
			Health = new Health(1);
			CharacterClass = new DnD5eCharacterClass();
			Race = new DnD5eCharacterRace();

			name = string.Empty;
			background = string.Empty;
			CharacterType = CharacterType.DnD5e;
		}

		public DnD5eCharacter(DnD5eCharacterClassData classData, DnD5eCharacterRaceData raceData, DnD5eBackgroundData backgroundData)
		{
			ToolProficiences = new ObservableCollection<string>();
			OtherProficiences = new ObservableCollection<string>();
			SpellBook = new SpellBook();
			NoteManager = new NoteBook();
			MovementTypes_Speeds = new ObservableCollection<Property>();
			Languages = new ObservableCollection<string>();
			abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
			Level = new CharacterLevel();
			Health = new Health(1);
			Inventory = new Inventory();

			CharacterClass = new DnD5eCharacterClass(classData);
			Race = new DnD5eCharacterRace(raceData);
			Background = backgroundData.Name;
			Size = raceData.Size;
			Alignment = Alignment;
			NoteManager.NewNoteSection(new NoteSection("Character"));
			NoteManager.GetSection("Character").Add(new Note(backgroundData.Name, backgroundData.Desc));
			AddMovementType(new Property(MovementType.WALK.ToString(), raceData.Speed));
			AddLanguages(raceData.Languages);
			WeaponProficiencies = new ObservableCollection<string>(classData.WeaponProficiencies);
			ArmorProficiencies = new ObservableCollection<string>(classData.ArmorProficiencies);

			name = string.Empty;
			background = string.Empty;
			CharacterType = CharacterType.DnD5e;
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
