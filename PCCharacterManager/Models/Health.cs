using Newtonsoft.Json;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Health : ObservableObject
	{
		private int currHealth;
		public int CurrHealth
		{
			get { return currHealth; }
			set
			{
				OnPropertyChanged(ref currHealth, value);
			}
		}

		private int maxHealth;
		[JsonProperty]
		public int MaxHealth
		{
			get { return maxHealth; }
			private set { OnPropertyChanged(ref maxHealth, value); }
		}

		private int tempHitPoints;
		public int TempHitPoints
		{
			get { return tempHitPoints; }
			set { OnPropertyChanged(ref tempHitPoints, value); }
		}

		public Health(int _maxHealth)
		{
			MaxHealth = _maxHealth;
			CurrHealth = _maxHealth;

			maxHealth = _maxHealth;
			currHealth = _maxHealth;

		}

		/// <summary>
		/// sets the max health 
		/// </summary>
		/// <param name="_maxHealth">value to set it to. must be at least 1</param>
		public void SetMaxHealth(int _maxHealth)
		{
			if (_maxHealth < 1)
				_maxHealth = 1;

			MaxHealth = _maxHealth;
			CurrHealth = maxHealth;
		}

		/// <summary>
		/// increates the current health level
		/// </summary>
		/// <param name="amount">amount to add to health</param>
		/// <exception cref="ArgumentOutOfRangeException">negative number passed</exception>
		public void Heal(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException();

			int temp = CurrHealth + amount;

			if (temp > MaxHealth)
				temp = MaxHealth;

			CurrHealth = temp;
		}

		/// <summary>
		/// reduces the current health level
		/// </summary>
		/// <param name="amount">amount to reduce health</param>
		/// <returns>true: health is <= 0, false: health is greater than 0</returns>
		/// <exception cref="ArgumentOutOfRangeException">negative number passed</exception>
		public bool Damage(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException();

			int temp = CurrHealth - amount;

			// no health needs to make saves
			if (temp <= 0)
			{
				CurrHealth = 0;
				return true;
			}

			CurrHealth = temp;
			return false;

		}
	}
}
