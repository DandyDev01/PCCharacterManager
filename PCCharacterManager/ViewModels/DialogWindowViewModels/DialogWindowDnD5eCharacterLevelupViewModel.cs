using Accessibility;
using PCCharacterManager.Services;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	public class DialogWindowDnD5eCharacterLevelupViewModel : ObservableObject
	{
		private readonly DialogServiceBase _dialogService;

		public DialogWindowDnD5eCharacterLevelupViewModel(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
		}

		private void AddClass()
		{
			throw new NotImplementedException();
		}

		private void IncreaseAbilityScore()
		{
			throw new NotImplementedException();
		}

		private void RollForMaxHealth()
		{
			throw new NotImplementedException();
		}

		public void ProcessLevelup()
		{
			throw new NotImplementedException();
		}
	}
}
