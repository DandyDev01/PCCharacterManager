using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	public class DialogWindowShortRestViewModel : ObservableObject
	{
		private readonly DnD5eCharacter _character;		

		private int _health;
		public int Health
		{
			get
			{
				return _health;
			}
			set
			{
				OnPropertyChanged(ref _health, value);
			}
		}


		private string _remainingRolls;
		public string RemainingRolls
		{
			get
			{
				return _remainingRolls;
			}
			set
			{
				OnPropertyChanged(ref _remainingRolls, value);
			}
		}

		private bool _canRoll;
		public bool CanRoll
		{
			get
			{
				return _canRoll;
			}
			set
			{
				OnPropertyChanged(ref _canRoll, value);
			}
		}

		private int _spentHitDice;
		public int SpentHitDice => _spentHitDice;

		private bool _hasHitDice;
		private bool _canGainHealth;

		public ICommand RollCommand { get; }

		public DialogWindowShortRestViewModel(DnD5eCharacter character)
		{
			_character = character;
			RollCommand = new RelayCommand(Roll);

			_health = character.Health.CurrHealth;
			_spentHitDice = character.SpentHitDie;
			
			_hasHitDice = character.SpentHitDie < character.Level.Level ? true : false;
			_canGainHealth = _health < _character.Health.MaxHealth ? true : false;
			_canRoll = _hasHitDice && _canGainHealth;
			
			_remainingRolls = "Remaining Rolls: " + (character.Level.Level - character.SpentHitDie) + "/" + character.Level.Level;
		}

		private void Roll()
		{
			RollDie die = new();

			int roll = die.Roll(_character.CharacterClass.HitDie);

			_spentHitDice += 1;

			Health += roll + _character.Abilities.Where(x => x.Skills.Count() <= 0).First().Modifier;

			Health = Math.Min(Health, _character.Health.MaxHealth);

			_hasHitDice = _spentHitDice < _character.Level.Level ? true : false;
			_canGainHealth = Health < _character.Health.MaxHealth ? true : false;	

			CanRoll = _hasHitDice && _canGainHealth;

			RemainingRolls = "Remaining Rolls: " + (_character.Level.Level - _spentHitDice) + "/" + _character.Level.Level;
		}
	}
}
