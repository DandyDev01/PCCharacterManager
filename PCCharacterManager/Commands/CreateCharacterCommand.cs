using PCCharacterManager.DialogWindows;
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

		public CreateCharacterCommand(CharacterStore _characterStore)
		{
			characterStore = _characterStore;
		}

		public override void Execute(object? parameter)
		{
			Window newCharacterWindow = new CreateCharacterDialogWindow();
			newCharacterWindow.DataContext = new DialogWindowCharacterCreaterViewModel(characterStore, newCharacterWindow);

			newCharacterWindow.ShowDialog();
		}
	}
}
