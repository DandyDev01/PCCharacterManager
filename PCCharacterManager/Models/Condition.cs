using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
    public class Condition : ObservableObject
	{
		private readonly Property _property;
		private int _durationInRounds;

		public string Name
		{
			get => _property.Name; 
			set => _property.Name = value;
		}
		public string Desc
		{
			get => _property.Desc;
			set => _property.Desc = value;
		}
		public string RoundsRemaning
		{
			get => "Rounds Remaining: " + (_durationInRounds - RoundsPassed);
		}
		public int DurationInRounds
		{
			get
			{
				return _durationInRounds;
			}
			set
			{
				OnPropertyChanged(ref _durationInRounds, value);
			}
		}

		public int RoundsPassed { get; private set; }

		public Condition(string name, string desc, int durationInRounds)
		{
			_property = new Property(name, desc);
			_durationInRounds = durationInRounds;
		}

		public void PassRound()
		{
			RoundsPassed += 1;
			OnPropertyChanged(nameof(RoundsRemaning));
		}
    }
}
