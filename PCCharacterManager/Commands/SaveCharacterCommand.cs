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
		private readonly ICharacterDataService dataService;
		private readonly CharacterStore characterStore;
		private readonly MainWindowViewModel mainWindowViewModel;

		public SaveCharacterCommand(MainWindowViewModel _mainWindowViewModel)
		{
			mainWindowViewModel = _mainWindowViewModel;
		}

		public SaveCharacterCommand(ICharacterDataService _dataService, CharacterStore _characterStore)
		{
			dataService = _dataService;
			characterStore = _characterStore;
		}

		public override void Execute(object parameter)
		{
			//dataService.Save(characterStore.SelectedCharacter);
			mainWindowViewModel.TabVM.CharacterListVM.SaveCharacter();
		}
	}
}
