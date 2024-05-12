using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class CreateCharacterCommand : BaseCommand
	{
		private readonly CharacterStore _characterStore;
		private readonly DialogServiceBase _dialogService;

		public CreateCharacterCommand(CharacterStore characterStore, DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			DialogWindowCharacterCreaterViewModel dataContext = new(_characterStore, _dialogService);

			string result = string.Empty;
			_dialogService.ShowDialog<CreateCharacterDialogWindow, DialogWindowCharacterCreaterViewModel>(dataContext, r =>
			{
				result = r;
			});
		}
	}
}
