using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class SimpleCharacterRecovery : RecoveryBase
	{
		private readonly List<DnD5eCharacter> _stateHistory;
		private int _stateIndex;

		public SimpleCharacterRecovery()
		{
			_stateHistory = new List<DnD5eCharacter>();
			_stateIndex = 0;
		}

		public DnD5eCharacter Redo()
		{
			int indexOfStateToReturn = _stateIndex + 1;

			if (indexOfStateToReturn >= _stateHistory.Count)
				indexOfStateToReturn = _stateHistory.Count - 1;
			else
				_stateIndex += 1;

			return _stateHistory[indexOfStateToReturn];
		}

		public DnD5eCharacter Undo()
		{
			int indexOfStateToReturn = _stateIndex - 1;

			if (indexOfStateToReturn < 0)
				indexOfStateToReturn = 0;
			else
				_stateIndex += 1;

			return _stateHistory[indexOfStateToReturn];
		}
		
		public void RegisterChange(DnD5eCharacter state)
		{
			_stateHistory.Add(state);
			_stateIndex += 1;
		}
	}
}
