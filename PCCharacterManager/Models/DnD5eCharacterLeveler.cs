using PCCharacterManager.Services;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterLeveler : CharacterLeveler
	{
		public DnD5eCharacterLeveler(DialogServiceBase dialogService) : base(dialogService)
		{
		}

		/// <summary>
		/// Opens a dialog window for the user to levelup the character.
		/// </summary>
		/// <param name="character">The character to level up</param>
		public override bool LevelCharacter(DnD5eCharacter character)
		{
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(_dialogService);

			string result = string.Empty;
			_dialogService.ShowDialog<DnD5eLevelupCharacterDialogWindow, 
				DialogWindowDnD5eCharacterLevelupViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return false;

			vm.ProcessLevelup();

			return true;
		}
	}
}
