using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class RollDie
	{
		public static readonly int[] DefaultAbilityScores = { 15, 14, 13, 12, 10, 8 };

		private readonly Random _random = new Random();

		/// <summary>
		/// rolls a d100
		/// </summary>
		/// <param name="numRolls">number of times to roll the d100</param>
		/// <returns>sum of all rolls</returns>
		public int D100(int numRolls = 1)
		{
			return Roll(100, numRolls);
		}

		/// <summary>
		/// rolls d20
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D20(int numRolls = 1)
		{
			return Roll(20, numRolls);
		}

		/// <summary>
		/// rolls d12
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D12(int numRolls = 1)
		{
			return Roll(12, numRolls);
		}

		/// <summary>
		/// rolls d10
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D10(int numRolls = 1)
		{
			return Roll(10, numRolls);
		}

		/// <summary>
		/// rolls d8
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D8(int numRolls = 1)
		{
			return Roll(8, numRolls);
		}

		/// <summary>
		/// rolls d6
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D6(int numRolls = 1)
		{
			return Roll(6, numRolls);
		}

		/// <summary>
		/// rolls d3
		/// </summary>
		/// <param name="numRolls">number of times to roll</param>
		/// <returns>sum of all rolls</returns>
		public int D3(int numRolls = 1)
		{
			return Roll(3, numRolls);
		}

		/// <summary>
		/// rolls a dice with a specified number of sides and specified number of rolls
		/// </summary>
		/// <param name="sides">number of sides the die has</param>
		/// <param name="times">number of times to roll the die defaults to 1</param>
		/// <returns>sum of all rolls</returns>
		public int Roll(int sides, int times = 1)
		{
			int total = 0;
			for (int i = 0; i < times; i++)
			{
				total += _random.Next(1, sides + 1);

			}
			return total;
		}

		public int Roll(HitDie hitdie, int times = 1)
		{
			int sides = (int.Parse(hitdie.ToString().Substring(hitdie.ToString().IndexOf('D') + 1)));
			return Roll(sides, times);
		}

		/// <summary>
		/// Rolls for an ability score
		/// </summary>
		/// <returns>the score of the abilty</returns>
		public int AbilityScoreRoll()
		{

			int[] rolls = new int[4];
			for (int i = 0; i < 4; i++)
			{
				rolls[i] = D6();
			}


			int total = 0;
			for (int i = 0; i < 3; i++)
			{
				total += GetMax(ref rolls);
			}

			return total;
		}

		/// <summary>
		/// finds the highest number in an array of ints
		/// </summary>
		/// <param name="rolls"></param>
		/// <returns>highest number</returns>
		private int GetMax(ref int[] rolls)
		{
			int max = 0;
			int temp;
			for (int i = 0; i < rolls.Length; i++)
			{
				if (rolls[i] > max)
				{
					temp = max;
					max = rolls[i];
					rolls[i] = temp;
				}
			}

			return max;
		}
	}
}
