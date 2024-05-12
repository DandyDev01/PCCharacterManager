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
	public class SelectCharacterCommand : BaseCommand
	{
		public string _characterPath;
		private readonly CharacterStore _characterStore;
		private readonly DialogServiceBase _dialogService;

		public SelectCharacterCommand(CharacterStore characterStore, string characterPath, DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_characterPath = characterPath;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			DnD5eCharacter? selectedCharacter;
			//Console.WriteLine("selected character is: " + characterToSelect.Name);
			if (_characterPath.Contains("dnd5e", StringComparison.OrdinalIgnoreCase))
			{
				selectedCharacter = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(_characterPath);
			}
			else if(_characterPath.Contains("starfinder", StringComparison.OrdinalIgnoreCase))
			{
				selectedCharacter = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(_characterPath);
			}
			else
			{
				_dialogService.ShowMessage("There is a problem with the character path you wish to select",
					"character select problem", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (selectedCharacter == null)
			{
				_dialogService.ShowMessage("There is a problem with the character you wish to select", 
					"character select problem", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			_characterStore.BindSelectedCharacter(selectedCharacter);
		}
	}
}
