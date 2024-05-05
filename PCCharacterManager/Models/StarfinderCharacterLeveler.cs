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
		protected override AddClassHelper AddClass(DnD5eCharacter character)
		{
			throw new NotImplementedException();
		}

		protected override void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel)
		{
			throw new NotImplementedException();
		}

		protected override bool UpdateMaxHealth(DnD5eCharacter character)
		{
			MessageBox.Show("Automatic Starfinder character leveling is not yet implemented.", 
				"Action Not Yet Supported.", MessageBoxButton.OK, MessageBoxImage.Information);

			return false;
		}
	}
}
