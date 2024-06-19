using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
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
    public abstract class CharacterBase : ObservableObject
    {
		public static DnD5eCharacter Default => new();

		protected string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				OnPropertyChanged(ref _name, value);
				OnCharacterChangedAction?.Invoke(this);
			}
		}

		protected string _id;
		public string Id
		{
			get => _id;
			set => OnPropertyChanged(ref _id, value);
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

		public virtual int CarryWeight { get; }

		public DnD5eCharacterClass CharacterClass { get; set; }
		public DnD5eCharacterRace Race { get; set; }
		public ArmorClass ArmorClass { get; set; }
		public CharacterLevel Level { get; set; }
		public NoteBook NoteManager { get; set; }
		public Inventory Inventory { get; set; }
		public SpellBook SpellBook { get; set; }
		public Health Health { get; set; }

		[JsonProperty(nameof(CharacterType))]
		[JsonConverter(typeof(StringEnumConverter))]
		public CharacterType CharacterType { get; set; }

		[JsonIgnore]
		public Action<CharacterBase>? OnCharacterChangedAction { get; set; }

		public CharacterBase()
		{

			ArmorClass = new ArmorClass();
			CharacterClass = new DnD5eCharacterClass();
			Race = new DnD5eCharacterRace();
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

			_id = string.Empty;
			_name = string.Empty;
			_dateModified = string.Empty;
			CharacterType = CharacterType.DnD5e;
		}

		public CharacterBase(DnD5eCharacterClassData classData, DnD5eCharacterRaceData raceData,
			DnD5eBackgroundData backgroundData)
		{

			CharacterClass = new DnD5eCharacterClass(classData);
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

			_id = string.Empty;
			_name = string.Empty;
			_dateModified = string.Empty;
			CharacterType = CharacterType.DnD5e;

			NoteManager.NewNoteSection(new NoteSection("Character"));
			NoteManager.GetSection("Character")!.Add(new Note(backgroundData.Name, backgroundData.Desc));
		}

		/// <summary>
		/// Used to notify when any aspect of the character changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCharacterChanged(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			OnCharacterChangedAction?.Invoke(this);
		}

		protected void OnCharacterChanged(object? sender, PropertyChangedEventArgs e)
		{
			OnCharacterChangedAction?.Invoke(this);
		}
	}
}
