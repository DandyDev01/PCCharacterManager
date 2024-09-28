using PCCharacterManager.Services;

namespace PCCharacterManager.Models
{
	public abstract class CharacterLeveler
	{
		protected DialogServiceBase _dialogService;

		public CharacterLeveler(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
		}

		public abstract bool LevelCharacter(CharacterBase character);
	}
}
