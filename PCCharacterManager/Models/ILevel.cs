namespace PCCharacterManager.Models
{
	public interface ILevel
	{
		int Level { get; set; }

		void LevelUp();
	}
}
