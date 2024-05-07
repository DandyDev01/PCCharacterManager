using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Models
{
	public abstract class CharacterLeveler
	{
		protected DialogServiceBase _dialogService;

		public CharacterLeveler(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
		}

		public abstract void LevelCharacter(DnD5eCharacter character);
	}
}
