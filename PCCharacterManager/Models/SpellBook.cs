using Newtonsoft.Json.Linq;
using PCCharacterManager.Helpers;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class SpellBook : ObservableObject
	{
		public Dictionary<SpellSchool, ObservableCollection<Spell>> SpellsKnown { get; private set; }
		public ObservableCollection<Spell> CantripsKnown { get; private set; }
		public ObservableCollection<Spell> PreparedSpells { get; private set; }

		public Note Note { get; private set; }

		#region Spell Slots
		public ObservableCollection<BoolHelper> FirstLvl { get; private set; }
		public ObservableCollection<BoolHelper> SecondLvl { get; private set; }
		public ObservableCollection<BoolHelper> ThirdLvl { get; private set; }
		public ObservableCollection<BoolHelper> FourthLvl { get; private set; }
		public ObservableCollection<BoolHelper> FifthLvl { get; private set; }
		public ObservableCollection<BoolHelper> SixLvl { get; private set; }
		public ObservableCollection<BoolHelper> SeventhLvl { get; private set; }
		public ObservableCollection<BoolHelper> EightLvl { get; private set; }
		public ObservableCollection<BoolHelper> NinethLvl { get; private set; }

		private int _slotNine;
		public int SlotNine
		{
			get { return _slotNine; }
			set
			{
				SpellSlotHelper(value, ref _slotNine, NinethLvl);
			}
		}

		private int _slotEight;
		public int SlotEight
		{
			get { return _slotEight; }
			set
			{
				SpellSlotHelper(value, ref _slotEight, EightLvl);
			}
		}

		private int _slotSeven;
		public int SlotSeven
		{
			get { return _slotSeven; }
			set
			{
				SpellSlotHelper(value, ref _slotSeven, SeventhLvl);
			}
		}

		private int _slotSix;
		public int SlotSix
		{
			get { return _slotSix; }
			set
			{
				SpellSlotHelper(value, ref _slotSix, SixLvl);
			}
		}

		private int _slotFive;
		public int SlotFive
		{
			get { return _slotFive; }
			set
			{
				SpellSlotHelper(value, ref _slotFive, FifthLvl);
			}
		}

		private int _slotFour;
		public int SlotFour
		{
			get { return _slotFour; }
			set
			{
				SpellSlotHelper(value, ref _slotFour, FourthLvl);
			}
		}

		private int _slotThree;
		public int SlotThree
		{
			get { return _slotThree; }
			set
			{
				SpellSlotHelper(value, ref _slotThree, ThirdLvl);
			}
		}

		private int _slotTwo;
		public int SlotTwo
		{
			get { return _slotTwo; }
			set
			{
				SpellSlotHelper(value, ref _slotTwo, SecondLvl);
			}
		}

		private int _slotOne;
		public int SlotOne
		{
			get { return _slotOne; }
			set
			{
				SpellSlotHelper(value, ref _slotOne, FirstLvl);

			}
		}
		#endregion

		public SpellBook()
		{
			SpellsKnown = new Dictionary<SpellSchool, ObservableCollection<Spell>>();

			foreach (SpellSchool item in Enum.GetValues(typeof(SpellSchool)))
			{
				SpellsKnown.Add(item, new ObservableCollection<Spell>());
			}

			CantripsKnown = new ObservableCollection<Spell>();
			PreparedSpells = new ObservableCollection<Spell>();

			Note = new Note();

			FirstLvl = new ObservableCollection<BoolHelper>();
			SecondLvl = new ObservableCollection<BoolHelper>();
			ThirdLvl = new ObservableCollection<BoolHelper>();
			FourthLvl = new ObservableCollection<BoolHelper>();
			FifthLvl = new ObservableCollection<BoolHelper>();
			SixLvl = new ObservableCollection<BoolHelper>();
			SeventhLvl = new ObservableCollection<BoolHelper>();
			EightLvl = new ObservableCollection<BoolHelper>();
			NinethLvl = new ObservableCollection<BoolHelper>();

		}

		/// <summary>
		/// Prepares a spell for use.
		/// </summary>
		/// <param name="spell">The spell that is getting prepared</param>
		public void PrepareSpell(Spell spell)
		{
			spell.IsPrepared = true;
			PreparedSpells.Add(spell);
		}

		/// <summary>
		/// Sets all prepared spell to no longer be prepared.
		/// </summary>
		public void ClearPreparedSpells()
		{
			foreach (var spell in PreparedSpells)
			{
				spell.IsPrepared = false;
			}

			PreparedSpells.Clear();
		}

		/// <summary>
		/// Add a spell.
		/// </summary>
		/// <param name="spell">The spell to add.</param>
		public void AddSpell(Spell spell)
		{
			SpellsKnown[spell.School].Add(spell);
		}

		/// <summary>
		/// Add a cantrip.
		/// </summary>
		/// <param name="spell">The cantrip to add.</param>
		public void AddContrip(Spell spell)
		{
			CantripsKnown.Add(spell);
		}

		/// <summary>
		/// Remove a spell.
		/// </summary>
		/// <param name="spell">The spell to remove.</param>
		public void RemoveSpell(Spell spell)
		{
			if (PreparedSpells.Contains(spell))
				PreparedSpells.Remove(spell);

			SpellsKnown[spell.School].Remove(spell);
		}

		/// <summary>
		/// Removes a cantrip.
		/// </summary>
		/// <param name="spell">Cantrip to remove.</param>
		public void RemoveCantrip(Spell spell)
		{
			CantripsKnown.Remove(spell);
		}

		/// <summary>
		/// Sets up the number of spell slots there should be.
		/// </summary>
		/// <param name="value">Number of spell slots we want</param>
		/// <param name="numberOfSlots">Number of spell slots there are</param>
		/// <param name="slots">Collection of slots</param>
		private void SpellSlotHelper(int value, ref int numberOfSlots, ObservableCollection<BoolHelper> slots)
		{
			if (value < 0) 
				value = 0;

			int temp = numberOfSlots;
			numberOfSlots = value;

			// add more spell slots
			if (value > temp)
			{
				for (int i = 0; i < (value - temp); i++)
				{
					slots.Add(new BoolHelper(false));
				}
			}
			// remove spell slots
			else if (value < temp)
			{
				for (int i = 0; i < (temp - value); i++)
				{
					slots.RemoveAt(slots.Count - 1);
				}
			}

			// there are more slots than there should be, remove the extras.
			if (slots.Count > numberOfSlots)
			{
				while (slots.Count > numberOfSlots)
				{
					slots.RemoveAt(slots.Count - 1);
				}
			}

			OnPropertyChanged(ref numberOfSlots, value);
		}

	} // end class
}
