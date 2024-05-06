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
		private readonly CharacterStore characterStore;
		private readonly DialogService _dialogService;

		public CreateCharacterCommand(CharacterStore _characterStore, DialogService dialogService)
		{
			characterStore = _characterStore;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			Window newCharacterWindow = new CreateCharacterDialogWindow();
			newCharacterWindow.DataContext = new DialogWindowCharacterCreaterViewModel(characterStore, newCharacterWindow,
				_dialogService);

			newCharacterWindow.ShowDialog();
		}
	}
}
