using PCCharacterManager.Models;
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
	public class DeleteCharacterCommand : BaseCommand
	{
		private readonly CharacterListViewModel _characterListVM;
		private readonly ICharacterDataService _dataService;
		private readonly CharacterStore _characterStore;
		private readonly DialogServiceBase _dialogService;

		public DeleteCharacterCommand(CharacterListViewModel characterListVM, ICharacterDataService dataService,
			CharacterStore characterStore, DialogServiceBase dialogService)
		{
			this._characterListVM = characterListVM;
			_dataService = dataService;
			_characterStore = characterStore;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			string resultsText = "are you sure you want to delete the character " + _characterStore.SelectedCharacter.Name + "?";
			string resultsCaption = "Delete Character";

			var results = _dialogService.ShowMessage(resultsText, resultsCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (results == MessageBoxResult.No)
				return;

			DnD5eCharacter character = _characterStore.SelectedCharacter;
			CharacterItemViewModel? item = _characterListVM.CharacterItems.First(x => x.CharacterName == character.Name);

			if (item == null)
			{
				string errorText = "no character with name " + _characterStore.SelectedCharacter.Name + " exists";
				string errorCaption = "Could not find Character";

				_dialogService.ShowMessage(errorText, errorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_dataService.Delete(_characterStore.SelectedCharacter);
			_characterListVM.CharacterItems.Remove(item);

			if (_characterListVM.CharacterItems.Count <= 0)
			{
				_characterListVM.CreateCharacterCommand.Execute(null);
				return;
			}

			_characterListVM.CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}
	}
}
