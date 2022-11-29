using Microsoft.Win32;
using PCCharacterManager.DialogWindows;
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
	public class CharacterExportCommand : BaseCommand
	{
		private readonly CharacterStore characterStore;
		private readonly TabControlViewModel tabVM;

		public CharacterExportCommand(CharacterStore _characterStore, TabControlViewModel _tabVM)
		{
			characterStore = _characterStore;
			tabVM = _tabVM;
		}

		public override void Execute(object parameter)
		{
			CharacterItemViewModel[] characterItems = tabVM.CharacterListVM.CharacterItems.ToArray();
			string[] characterNames = new string[characterItems.Length];
			string savePath = string.Empty;
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "Json files|*.json";
			saveFile.FileName = "PCManager_Export.json";

			// get all character names
			for (int i = 0; i < characterItems.Length; i++)
			{
				characterNames[i] = characterItems[i].CharacterName;
			}

			// select characters to export
			Window selectCharactersWindow = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue dataContext =
				new DialogWindowSelectStingValue(selectCharactersWindow,
				characterNames, characterNames.Length);

			selectCharactersWindow.DataContext = dataContext;
			selectCharactersWindow.ShowDialog();

			if ((bool)!selectCharactersWindow.DialogResult) return;

			string[] selectedCharacterNames = dataContext.SelectedItems.ToArray();
			string[] characterPaths = new string[selectedCharacterNames.Length];

			// select where to save export files
			var fileDialogResult = saveFile.ShowDialog();

			if ((bool)!fileDialogResult) return;

			savePath = saveFile.FileName;

			var messageBoxResult = MessageBox.Show("Single file export? (yes) for 1.json file (no) for individual .json files",
				"single or multiple files", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Cancel) return;
			else if (messageBoxResult == MessageBoxResult.Yes)
				SingleFileExport(characterItems, savePath, selectedCharacterNames, characterPaths);
			else if (messageBoxResult == MessageBoxResult.No)
				MultiFileExport(characterItems, savePath, selectedCharacterNames, characterPaths);
		}

		/// <summary>
		/// export the characters to a single file
		/// </summary>
		/// <param name="characterItems"></param>
		/// <param name="savePath"></param>
		/// <param name="selectedCharacterNames"></param>
		/// <param name="characterPaths"></param>
		private void SingleFileExport(CharacterItemViewModel[] characterItems, string savePath, string[] selectedCharacterNames, string[] characterPaths)
		{
			for (int i = 0; i < selectedCharacterNames.Length; i++)
			{
				foreach (var item in characterItems)
				{
					if (item.CharacterName.Equals(selectedCharacterNames[i])) characterPaths[i] = item.CharacterPath;
				}
			}

			Character[] characters = new Character[characterPaths.Length];

			for (int i = 0; i < characters.Length; i++)
			{
				// selectedCharacter is to be exported
				if (characterPaths[i].Contains(characterStore.SelectedCharacter.Name))
				{
					characters[i] = characterStore.SelectedCharacter;
					continue;
				}
				characters[i] = ReadWriteJsonFile<Character>.ReadFile(characterPaths[i]);
			}

			ReadWriteJsonCollection<Character>.WriteCollection(savePath, characters);
		}

		/// <summary>
		/// eport characters to seperate files
		/// </summary>
		/// <param name="characterItems"></param>
		/// <param name="savePath"></param>
		/// <param name="selectedCharacterNames"></param>
		/// <param name="characterPaths"></param>
		private void MultiFileExport(CharacterItemViewModel[] characterItems, string savePath, string[] selectedCharacterNames, string[] characterPaths)
		{
			for (int i = 0; i < selectedCharacterNames.Length; i++)
			{
				foreach (var item in characterItems)
				{
					if (item.CharacterName.Equals(selectedCharacterNames[i])) characterPaths[i] = item.CharacterPath;
				}
			}

			for (int i = 0; i < characterPaths.Length; i++)
			{
				savePath = savePath.Substring(0, savePath.IndexOf('.'));
				if (characterPaths[i].Contains(characterStore.SelectedCharacter.Name))
				{
					ReadWriteJsonFile<Character>.WriteFile(savePath + "_" + characterStore.SelectedCharacter.Name + ".json", characterStore.SelectedCharacter);
					savePath += ".json";
					continue;
				}
				Character character = ReadWriteJsonFile<Character>.ReadFile(characterPaths[i]);
				ReadWriteJsonFile<Character>.WriteFile(savePath + "_" + character.Name + ".json", character);
				savePath += ".json";
			}
		}
	}
}
