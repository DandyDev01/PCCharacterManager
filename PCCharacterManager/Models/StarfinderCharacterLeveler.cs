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
		public StarfinderCharacterLeveler(DialogService dialogService) : base(dialogService)
		{
		}

		protected override AddClassHelper AddClass(DnD5eCharacter character, string classToAddName)
		{
			throw new NotImplementedException();
		}

		protected override bool MeetsPrerequisites(DnD5eCharacter character, CharacterMultiClassData characterMultiClassData)
		{
			throw new NotImplementedException();
		}

		protected override void UnLockClassFeatures(DnD5eCharacter character, string className, int classLevel)
		{
			throw new NotImplementedException();
		}

		protected override MultiClass UpdateMaxHealth(DnD5eCharacter character)
		{
			MessageBox.Show("Automatic Starfinder character leveling is not yet implemented.", 
				"Action Not Yet Supported.", MessageBoxButton.OK, MessageBoxImage.Information);

			return new MultiClass();
		}
	}
}
