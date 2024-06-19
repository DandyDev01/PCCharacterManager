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
		private readonly MainWindowViewModel _mainWindowViewModel;

		public SaveCharacterCommand(MainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}
		public override void Execute(object? parameter)
		{
			_mainWindowViewModel.TabVM.CharacterListVM.SaveCharacter();
		}
	}
}
