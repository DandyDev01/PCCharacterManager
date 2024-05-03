using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClassLevel : ObservableObject, ILevel
	{
		private int _level;

		public int Level
		{
			get { return _level; }
			set { OnPropertyChanged(ref _level, value); }
		}

		public void LevelUp()
		{
			Level++;
		}
	}
}
