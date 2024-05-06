using Microsoft.Win32;
using PCCharacterManager.DialogWindows;
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
	public class CharacterExportCommand : BaseCommand
	{
		private DialogService _dialogService;
		private readonly CharacterStore _characterStore;
		private readonly TabControlViewModel _tabVM;

		public CharacterExportCommand(CharacterStore characterStore, TabControlViewModel tabVM, 
			DialogService dialogService)
		{
			_characterStore = characterStore;
			_tabVM = tabVM;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			CharacterItemViewModel[] characterItems = _tabVM.CharacterListVM.CharacterItems.ToArray();
			string[] characterNames = new string[characterItems.Length];
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "Json files|*.json";
			saveFile.FileName = "PCManager_Export.json";

			// get all character names
			for (int i = 0; i < characterItems.Length; i++)
			{
				characterNames[i] = characterItems[i].CharacterName;
			}

			// select characters to export
			DialogWindowSelectStingValueViewModel dataContext =
				new DialogWindowSelectStingValueViewModel(characterNames, characterNames.Length);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, 
				DialogWindowSelectStingValueViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			string[] selectedCharacterNames = dataContext.SelectedItems.ToArray();
			string[] characterPaths = new string[selectedCharacterNames.Length];

			// select where to save export files
			var fileDialogResult = saveFile.ShowDialog();

			if (fileDialogResult.GetValueOrDefault() == false) 
				return;

			string savePath = saveFile.FileName;

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
		private void SingleFileExport(CharacterItemViewModel[] characterItems, string savePath, 
			string[] selectedCharacterNames, string[] characterPaths)
		{
			DnD5eCharacter[] characters = new DnD5eCharacter[characterPaths.Length];
			
			// get path's of characters to export
			for (int i = 0; i < selectedCharacterNames.Length; i++)
			{
				foreach (var item in characterItems)
				{
					if (item.CharacterName.Equals(selectedCharacterNames[i])) 
						characterPaths[i] = item.CharacterPath;
				}
			}

			// write .json file for each character being exported
			for (int i = 0; i < characters.Length; i++)
			{
				// selectedCharacter is to be exported
				if (characterPaths[i].Contains(_characterStore.SelectedCharacter.Name))
				{
					characters[i] = _characterStore.SelectedCharacter;
					continue;
				}

				var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterPaths[i]) 
					?? throw new Exception("The characater at " + characterPaths[i] + " does not exist.");
				
				characters[i] = character;
			}

			ReadWriteJsonCollection<DnD5eCharacter>.WriteCollection(savePath, characters);
		}

		/// <summary>
		/// export characters to separate files
		/// </summary>
		/// <param name="characterItems"></param>
		/// <param name="savePath"></param>
		/// <param name="selectedCharacterNames"></param>
		/// <param name="characterPaths"></param>
		private void MultiFileExport(CharacterItemViewModel[] characterItems, string savePath, 
			string[] selectedCharacterNames, string[] characterPaths)
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
				if (characterPaths[i].Contains(_characterStore.SelectedCharacter.Name))
				{
					ReadWriteJsonFile<DnD5eCharacter>.WriteFile(savePath + "_" + _characterStore.SelectedCharacter.Name 
						+ ".json", _characterStore.SelectedCharacter);
					savePath += ".json";
					continue;
				}
				DnD5eCharacter? character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterPaths[i]);

				if (character is null)
				{
					throw new Exception("Character path " + characterPaths[i] + " does not exist.");
				}

				ReadWriteJsonFile<DnD5eCharacter>.WriteFile(savePath + "_" + character.Name + ".json", character);
				savePath += ".json";
			}
		}
	}
}
