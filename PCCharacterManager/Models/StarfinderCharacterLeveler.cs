using PCCharacterManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Models
{
	class StarfinderCharacterLeveler : CharacterLeveler
	{
		public StarfinderCharacterLeveler(DialogServiceBase dialogService) : base(dialogService)
		{
		}

		public override void LevelCharacter(DnD5eCharacter character)
		{
			MessageBox.Show("Automatic Starfinder character leveling is not yet implemented.",
				"Action Not Yet Supported.", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
