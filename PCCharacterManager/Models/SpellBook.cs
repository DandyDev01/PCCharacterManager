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

		private int slotNine;
		public int SlotNine
		{
			get { return slotNine; }
			set
			{
				SpellSlotHelper(value, ref slotNine, NinethLvl);
			}
		}

		private int slotEight;
		public int SlotEight
		{
			get { return slotEight; }
			set
			{
				SpellSlotHelper(value, ref slotEight, EightLvl);
			}
		}

		private int slotSeven;
		public int SlotSeven
		{
			get { return slotSeven; }
			set
			{
				SpellSlotHelper(value, ref slotSeven, SeventhLvl);
			}
		}

		private int slotSix;
		public int SlotSix
		{
			get { return slotSix; }
			set
			{
				SpellSlotHelper(value, ref slotSix, SixLvl);
			}
		}

		private int slotFive;
		public int SlotFive
		{
			get { return slotFive; }
			set
			{
				SpellSlotHelper(value, ref slotFive, FifthLvl);
			}
		}

		private int slotFour;
		public int SlotFour
		{
			get { return slotFour; }
			set
			{
				SpellSlotHelper(value, ref slotFour, FourthLvl);
			}
		}

		private int slotThree;
		public int SlotThree
		{
			get { return slotThree; }
			set
			{
				SpellSlotHelper(value, ref slotThree, ThirdLvl);
			}
		}

		private int slotTwo;
		public int SlotTwo
		{
			get { return slotTwo; }
			set
			{
				SpellSlotHelper(value, ref slotTwo, SecondLvl);
			}
		}

		private int slotOne;
		public int SlotOne
		{
			get { return slotOne; }
			set
			{
				SpellSlotHelper(value, ref slotOne, FirstLvl);

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
