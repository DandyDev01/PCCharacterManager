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
		private readonly CharacterListViewModel characterListVM;
		private readonly ICharacterDataService dataService;
		private readonly CharacterStore characterStore;

		public DeleteCharacterCommand(CharacterListViewModel _characterListVM, ICharacterDataService _dataService,
			CharacterStore _characterStore)
		{
			characterListVM = _characterListVM;
			dataService = _dataService;
			characterStore = _characterStore;
		}

		public override void Execute(object parameter)
		{
			if (characterStore.SelectedCharacter == null)
				return;

			string resultsText = "are you sure you want to delete the character " + characterStore.SelectedCharacter.Name + "?";
			string resultsCaption = "Delete Character";

			var results = MessageBox.Show(resultsText, resultsCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (results == MessageBoxResult.No)
				return;

			DnD5eCharacter character = characterStore.SelectedCharacter;
			CharacterItemViewModel? item = characterListVM.CharacterItems.First(x => x.CharacterName == character.Name);

			if (item == null)
			{
				string errorText = "no character with name " + characterStore.SelectedCharacter.Name + " exists";
				string errorCaption = "Could not find Character";

				MessageBox.Show(errorText, errorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			dataService.Delete(characterStore.SelectedCharacter);

			if (characterListVM.CharacterItems.Count <= 0)
			{
				characterListVM.CreateCharacterCommand.Execute(null);
				return;
			}

			characterListVM.CharacterItems.Remove(item);
			characterListVM.CharacterItems[0].SelectCharacterCommand?.Execute(null);
		}
	}
}
