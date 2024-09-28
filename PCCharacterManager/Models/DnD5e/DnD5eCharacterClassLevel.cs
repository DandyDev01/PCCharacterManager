using PCCharacterManager.Utility;

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
