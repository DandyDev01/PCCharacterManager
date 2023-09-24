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
				Helper(value, ref slotNine, NinethLvl);
			}
		}

		private int slotEight;
		public int SlotEight
		{
			get { return slotEight; }
			set
			{
				Helper(value, ref slotEight, EightLvl);
			}
		}

		private int slotSeven;
		public int SlotSeven
		{
			get { return slotSeven; }
			set
			{
				Helper(value, ref slotSeven, SeventhLvl);
			}
		}

		private int slotSix;
		public int SlotSix
		{
			get { return slotSix; }
			set
			{
				Helper(value, ref slotSix, SixLvl);
			}
		}

		private int slotFive;
		public int SlotFive
		{
			get { return slotFive; }
			set
			{
				Helper(value, ref slotFive, FifthLvl);
			}
		}

		private int slotFour;
		public int SlotFour
		{
			get { return slotFour; }
			set
			{
				Helper(value, ref slotFour, FourthLvl);
			}
		}

		private int slotThree;
		public int SlotThree
		{
			get { return slotThree; }
			set
			{
				Helper(value, ref slotThree, ThirdLvl);
			}
		}

		private int slotTwo;
		public int SlotTwo
		{
			get { return slotTwo; }
			set
			{
				Helper(value, ref slotTwo, SecondLvl);
			}
		}

		private int slotOne;
		public int SlotOne
		{
			get { return slotOne; }
			set
			{
				Helper(value, ref slotOne, FirstLvl);

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
		/// prepares a spell for use
		/// </summary>
		/// <param name="spell">The spell that is getting prepared</param>
		public void PrepareSpell(Spell spell)
		{
			spell.IsPrepared = true;
			PreparedSpells.Add(spell);
		}

		public void ClearPreparedSpells()
		{
			foreach (var spell in PreparedSpells)
			{
				spell.IsPrepared = false;
			}

			PreparedSpells.Clear();
		}

		public void AddSpell(Spell spell)
		{
			SpellsKnown[spell.School].Add(spell);
		}

		public void AddContrip(Spell spell)
		{
			CantripsKnown.Add(spell);
		}

		public void RemoveSpell(Spell spell)
		{
			if (PreparedSpells.Contains(spell))
				PreparedSpells.Remove(spell);

			SpellsKnown[spell.School].Remove(spell);
		}

		public void RemoveCantrip(Spell spell)
		{
			CantripsKnown.Remove(spell);
		}

		private void Helper(int value, ref int numberOfSlots, ObservableCollection<BoolHelper> slots)
		{
			if (value < 0) value = 0;

			int temp = numberOfSlots;
			numberOfSlots = value;

			if (value > temp)
			{
				for (int i = 0; i < (value - temp); i++)
				{
					slots.Add(new BoolHelper(false));
				}
			}
			else if (value < temp)
			{
				for (int i = 0; i < (temp - value); i++)
				{
					slots.RemoveAt(slots.Count - 1);
				}
			}
			// for some reason things are added w/out add being called
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
