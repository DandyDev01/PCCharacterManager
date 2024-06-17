using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class DarkSoulsCharacterCreatorViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
	{
		private readonly DialogServiceBase _dialogService;

		public bool HasErrors => false;

		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

		public DarkSoulsCharacterCreatorViewModel(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
		}

		public override DnD5eCharacter Create()
		{
			throw new NotImplementedException();
		}

		public IEnumerable GetErrors(string? propertyName)
		{
			throw new NotImplementedException();
		}
	}
}
 