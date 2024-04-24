using Microsoft.Win32;
using PCCharacterManager.Models;
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
		private readonly CharacterStore characterStore;

		public OpenCharacterCommand(CharacterStore _characterStore)
		{
			characterStore = _characterStore;
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

				characterStore.BindSelectedCharacter(character);
			}
			catch
			{
				MessageBox.Show("selected file did not contain PCManager character",
					"selected file was not a PCManager character.", MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}
	}
}
