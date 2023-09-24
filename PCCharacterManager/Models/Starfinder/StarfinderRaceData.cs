namespace PCCharacterManager.Models
{
	public class StarfinderRaceData : DnD5eCharacterRaceData
	{
		public int HitPoints { get; set; } = 0;
		public string HomeWorld { get; set; } = string.Empty;
	}
}