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
			DnD5eCharacter? selectedCharacter;
			//Console.WriteLine("selected character is: " + characterToSelect.Name);
			if (characterPath.Contains("dnd5e", StringComparison.OrdinalIgnoreCase))
			{
				selectedCharacter = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterPath);
			}
			else if(characterPath.Contains("starfinder", StringComparison.OrdinalIgnoreCase))
			{
				selectedCharacter = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(characterPath);
			}
			else
			{
				MessageBox.Show("there is a problem with the character you wish to select", "character select problem", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			characterStore.BindSelectedCharacter(selectedCharacter);

		}
	}
}
