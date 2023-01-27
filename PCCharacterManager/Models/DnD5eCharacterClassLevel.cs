﻿using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClassLevel : ObservableObject, ILevel
	{
		private int level;

		public int Level
		{
			get { return level; }
			set { OnPropertyChanged(ref level, value); }
		}

		public void LevelUp()
		{
			Level++;
		}
	}
}