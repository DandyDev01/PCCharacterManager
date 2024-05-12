using Microsoft.Win32;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class OpenCharacterCommand : BaseCommand
	{
		private readonly CharacterStore _characterStore;
		private readonly DialogServiceBase _dialogService;

		public OpenCharacterCommand(CharacterStore characterStore, DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			OpenFileDialog openFile = new();
			openFile.Filter = "Json file|*.json";
			openFile.ShowDialog();
			string path = openFile.FileName;
			try
			{
				DnD5eCharacter? character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(path) 
					?? throw new Exception("Character with path " + path + " does not exist.");

				_characterStore.BindSelectedCharacter(character);
			}
			catch
			{
				_dialogService.ShowMessage("selected file did not contain PCManager character",
					"selected file was not a PCManager character.", MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}
	}
}
