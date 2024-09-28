using PCCharacterManager.Services;
using System.Windows;

namespace PCCharacterManager.Models
{
	class StarfinderCharacterLeveler : CharacterLeveler
	{
		public StarfinderCharacterLeveler(DialogServiceBase dialogService) : base(dialogService)
		{
		}

		public override bool LevelCharacter(CharacterBase character)
		{
			MessageBox.Show("Automatic Starfinder character leveling is not yet implemented.",
				"Action Not Yet Supported.", MessageBoxButton.OK, MessageBoxImage.Information);

			return false;
		}
	}
}
