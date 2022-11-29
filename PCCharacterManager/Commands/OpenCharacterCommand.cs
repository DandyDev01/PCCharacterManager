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

		public override void Execute(object parameter)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "Json file|*.json";
			openFile.ShowDialog();
			string path = openFile.FileName;
			try
			{
				Character c = ReadWriteJsonFile<Character>.ReadFile(path);
				characterStore.BindSelectedCharacter(c);

			}
			catch (Exception ex)
			{
				MessageBox.Show("selected file did not contain PCManager character",
					"selected file was not a PCManager character.", MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}
	}
}
