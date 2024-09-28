namespace PCCharacterManager.Models
{
	public interface RecoveryBase
	{
		public DnD5eCharacter Undo();

		public DnD5eCharacter Redo();

		public void RegisterChange(CharacterBase state);

		public void ClearHistory();
	}
}
