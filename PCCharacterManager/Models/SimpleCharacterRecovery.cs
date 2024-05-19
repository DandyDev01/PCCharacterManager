﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class SimpleCharacterRecovery : RecoveryBase
	{
		private readonly List<string> _stateHistory;
		private int _stateIndex;

		public SimpleCharacterRecovery()
		{
			_stateHistory = new List<string>();
			_stateIndex = 0;
		}

		public DnD5eCharacter Redo()
		{
			int indexOfStateToReturn = _stateIndex + 1;

			if (indexOfStateToReturn >= _stateHistory.Count)
				indexOfStateToReturn = _stateHistory.Count - 1;
			else
				_stateIndex += 1;

			var objectType = JsonConvert.DeserializeObject<DnD5eCharacter>(_stateHistory[indexOfStateToReturn]);

			if (objectType == null)
				throw new Exception("error");

			return objectType;
		}

		public DnD5eCharacter Undo()
		{
			int indexOfStateToReturn = _stateIndex - 1;

			if (indexOfStateToReturn < 0)
				indexOfStateToReturn = 0;
			else
				_stateIndex -= 1;

			var objectType = JsonConvert.DeserializeObject<DnD5eCharacter>(_stateHistory[indexOfStateToReturn]);

			if (objectType == null)
				throw new Exception("error");

			return objectType;
		}
		
		public void RegisterChange(DnD5eCharacter state)
		{
			string json = JsonConvert.SerializeObject(state);
			_stateHistory.Add(json);
			_stateIndex += 1;
		}
	}
}