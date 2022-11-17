using PCCharacterManager.Models;
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
		private readonly string characterPath;
		private readonly CharacterStore characterStore;

		public SelectCharacterCommand(CharacterStore characterStore, string _characterPath)
		{
			this.characterStore = characterStore;
			characterPath = _characterPath;
		}

		public override void Execute(object parameter)
		{
			//Console.WriteLine("selected character is: " + characterToSelect.Name);
			Character? selectedCharacter = ReadWriteJsonFile<Character>.ReadFile(characterPath);

			if(selectedCharacter == null)
			{
				MessageBox.Show("there is a problem with the character you wish to select", "character select problem", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			characterStore.CharacterChange(selectedCharacter);

		}
	}
}
