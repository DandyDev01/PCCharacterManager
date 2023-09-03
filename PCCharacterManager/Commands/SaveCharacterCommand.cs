using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class SaveCharacterCommand : BaseCommand
	{
		private readonly MainWindowViewModel mainWindowViewModel;

		public SaveCharacterCommand(MainWindowViewModel _mainWindowViewModel)
		{
			mainWindowViewModel = _mainWindowViewModel;
		}
		public override void Execute(object? parameter)
		{
			//dataService.Save(characterStore.SelectedCharacter);
			mainWindowViewModel.TabVM.CharacterListVM.SaveCharacter();
		}
	}
}
