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
		public ObservableCollection<Spell> SpellsKnown { get; private set; }
		public ObservableCollection<Spell> CantripsKnown { get; private set; }
		public ObservableCollection<Spell> PreparedSpells { get; private set; }

		public Note Note { get; private set; }

		#region Spell Slots
		public ObservableCollection<bool> FirstLvl { get; private set; }
		public ObservableCollection<bool> SecondLvl { get; private set; }
		public ObservableCollection<bool> ThirdLvl { get; private set; }
		public ObservableCollection<bool> FourthLvl { get; private set; }
		public ObservableCollection<bool> FifthLvl { get; private set; }
		public ObservableCollection<bool> SixLvl { get; private set; }
		public ObservableCollection<bool> SeventhLvl { get; private set; }
		public ObservableCollection<bool> EightLvl { get; private set; }
		public ObservableCollection<bool> NinethLvl { get; private set; }

		private int slotNine;
		public int SlotNine
		{
			get { return slotNine; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotNine;
				slotNine = value;

				if (value > slotNine)
				{
					for (int i = 0; i < (value - slotNine); i++)
					{
						NinethLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (slotNine - value); i++)
					{
						NinethLvl.RemoveAt(NinethLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (NinethLvl.Count() > slotNine)
				{
					while (SixLvl.Count() > slotNine)
					{
						NinethLvl.RemoveAt(NinethLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotNine, value);
			}
		}

		private int slotEight;
		public int SlotEight
		{
			get { return slotEight; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotEight;
				slotEight = value;

				if (value > slotEight)
				{
					for (int i = 0; i < (value - slotEight); i++)
					{
						EightLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (slotEight - value); i++)
					{
						EightLvl.RemoveAt(EightLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (EightLvl.Count() > slotEight)
				{
					while (SixLvl.Count() > slotEight)
					{
						EightLvl.RemoveAt(EightLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotEight, value);
			}
		}

		private int slotSeven;
		public int SlotSeven
		{
			get { return slotSeven; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotSeven;
				slotSeven = value;

				if (value > slotSeven)
				{
					for (int i = 0; i < (value - slotSeven); i++)
					{
						SeventhLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (slotSeven - value); i++)
					{
						SeventhLvl.RemoveAt(SeventhLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (SeventhLvl.Count() > slotSeven)
				{
					while (SeventhLvl.Count() > slotSeven)
					{
						SeventhLvl.RemoveAt(SeventhLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotSeven, value);
			}
		}

		private int slotSix;
		public int SlotSix
		{
			get { return slotSix; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotSix;
				slotSix = value;

				if (value > slotSix)
				{
					for (int i = 0; i < (value - slotSix); i++)
					{
						SixLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (slotSix - value); i++)
					{
						SixLvl.RemoveAt(SixLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (SixLvl.Count() > slotSix)
				{
					while (SixLvl.Count() > slotSix)
					{
						SixLvl.RemoveAt(SixLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotSix, value);
			}
		}

		private int slotFive;
		public int SlotFive
		{
			get { return slotFive; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotFive;
				slotFive = value;

				if (value > temp)
				{
					for (int i = 0; i < (value - temp); i++)
					{
						FifthLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (temp - value); i++)
					{
						FifthLvl.RemoveAt(FifthLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (FifthLvl.Count() > slotFive)
				{
					while (FifthLvl.Count() > slotFive)
					{
						FifthLvl.RemoveAt(FifthLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotFive, value);
			}
		}

		private int slotFour;
		public int SlotFour
		{
			get { return slotFour; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotFour;
				slotFour = value;

				if (value > temp)
				{
					for (int i = 0; i < (value - temp); i++)
					{
						FourthLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (temp - value); i++)
					{
						FourthLvl.RemoveAt(FourthLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (FourthLvl.Count() > slotFour)
				{
					while (FourthLvl.Count() > slotFour)
					{
						FourthLvl.RemoveAt(FourthLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotFour, value);
			}
		}

		private int slotThree;
		public int SlotThree
		{
			get { return slotThree; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotThree;
				slotThree = value;

				if (value > temp)
				{
					for (int i = 0; i < (value - temp); i++)
					{
						ThirdLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (temp - value); i++)
					{
						ThirdLvl.RemoveAt(ThirdLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (ThirdLvl.Count() > slotThree)
				{
					while (ThirdLvl.Count() > slotThree)
					{
						ThirdLvl.RemoveAt(ThirdLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotThree, value);
			}
		}

		private int slotTwo;
		public int SlotTwo
		{
			get { return slotTwo; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotTwo;
				slotTwo = value;

				if (value > temp)
				{
					for (int i = 0; i < (value - temp); i++)
					{
						SecondLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (temp - value); i++)
					{
						SecondLvl.RemoveAt(SecondLvl.Count() - 1);
					}
				}
				// for some reason things are added w/out add being called
				if (SecondLvl.Count() > slotTwo)
				{
					while (SecondLvl.Count() > slotTwo)
					{
						SecondLvl.RemoveAt(SecondLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotTwo, value);
			}
		}

		private int slotOne;
		public int SlotOne
		{
			get { return slotOne; }
			set
			{
				if (value < 0) value = 0;

				int temp = slotOne;
				slotOne = value;

				//adding
				if (value > temp)
				{
					Console.WriteLine("things being added");
					for (int i = 0; i < (value - temp); i++)
					{
						FirstLvl.Add(false);
					}
				}
				else if (value < temp)
				{
					for (int i = 0; i < (temp - value); i++)
					{
						FirstLvl.RemoveAt(FirstLvl.Count() - 1);
					}
				}

				// for some reason things are added w/out add being called
				if (FirstLvl.Count() > slotOne)
				{
					while (FirstLvl.Count() > slotOne)
					{
						FirstLvl.RemoveAt(FirstLvl.Count() - 1);
					}
				}
				OnPropertyChaged(ref slotOne, value);

			}
		}
		#endregion

		public SpellBook()
		{
			SpellsKnown = new ObservableCollection<Spell>();
			CantripsKnown = new ObservableCollection<Spell>();
			PreparedSpells = new ObservableCollection<Spell>();

			Note = new Note();

			FirstLvl = new ObservableCollection<bool>();
			SecondLvl = new ObservableCollection<bool>();
			ThirdLvl = new ObservableCollection<bool>();
			FourthLvl = new ObservableCollection<bool>();
			FifthLvl = new ObservableCollection<bool>();
			SixLvl = new ObservableCollection<bool>();
			SeventhLvl = new ObservableCollection<bool>();
			EightLvl = new ObservableCollection<bool>();
			NinethLvl = new ObservableCollection<bool>();

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

		public void CastSpell(Spell spell)
		{
			PreparedSpells.Remove(spell);
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
			SpellsKnown.Add(spell);
		}

		public void AddContrip(Spell spell)
		{
			CantripsKnown.Add(spell);
		}

		public void RemoveSpell(Spell spell)
		{
			if (PreparedSpells.Contains(spell))
				PreparedSpells.Remove(spell);

			SpellsKnown.Remove(spell);
		}

		public void RemoveCantrip(Spell spell)
		{
			CantripsKnown.Remove(spell);
		}
	} // end class
}
