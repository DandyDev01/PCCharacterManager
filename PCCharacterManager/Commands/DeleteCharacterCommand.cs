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
		private readonly CharacterListViewModel _characterListViewModel;
		private readonly ICharacterDataService _dataService;
		private readonly CharacterStore _characterStore;
		private readonly DialogServiceBase _dialogService;

		public DeleteCharacterCommand(CharacterListViewModel characterListViewModel, ICharacterDataService dataService,
			CharacterStore characterStore, DialogServiceBase dialogService)
		{
			this._characterListViewModel = characterListViewModel;
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
			CharacterItemViewModel? item = _characterListViewModel.CharacterItems.First(x => x.CharacterName == character.Name);

			if (item == null)
			{
				string errorText = "no character with name " + _characterStore.SelectedCharacter.Name + " exists";
				string errorCaption = "Could not find Character";

				_dialogService.ShowMessage(errorText, errorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_dataService.Delete(_characterStore.SelectedCharacter);
			_characterListViewModel.CharacterItems.Remove(item);

			if (_characterListViewModel.CharacterItems.Count <= 0)
			{
				_characterListViewModel.CreateCharacterCommand.Execute(null);
				return;
			}

			_characterListViewModel.CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}
	}
}
