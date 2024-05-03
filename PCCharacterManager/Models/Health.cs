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
		private int _currHealth;
		public int CurrHealth
		{
			get { return _currHealth; }
			set
			{
				OnPropertyChanged(ref _currHealth, value);
			}
		}

		private int _maxHealth;
		public int MaxHealth
		{
			get { return _maxHealth; }
			set { OnPropertyChanged(ref _maxHealth, value); }
		}

		private int _tempHitPoints;
		public int TempHitPoints
		{
			get { return _tempHitPoints; }
			set { OnPropertyChanged(ref _tempHitPoints, value); }
		}

		public Health(int maxHealth)
		{
			MaxHealth = maxHealth;
			CurrHealth = maxHealth;

			this._maxHealth = maxHealth;
			_currHealth = maxHealth;

		}

		/// <summary>
		/// sets the max health 
		/// </summary>
		/// <param name="maxHealth">value to set it to. must be at least 1</param>
		public void SetMaxHealth(int maxHealth)
		{
			if (maxHealth < 1)
				maxHealth = 1;

			MaxHealth = maxHealth;
			CurrHealth = this._maxHealth;
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
