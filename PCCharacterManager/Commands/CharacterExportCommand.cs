﻿using Microsoft.Win32;
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
		private readonly DialogServiceBase _dialogService;
		private readonly CharacterStore _characterStore;
		private readonly TabControlViewModel _tabControlViewModel;

		public CharacterExportCommand(CharacterStore characterStore, TabControlViewModel tabControlViewModel, 
			DialogServiceBase dialogService)
		{
			_characterStore = characterStore;
			_tabControlViewModel = tabControlViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			CharacterItemViewModel[] characterItems = _tabControlViewModel.CharacterListVM.CharacterItems.ToArray();
			string[] characterNames = new string[characterItems.Length];
			SaveFileDialog saveFile = new();
			saveFile.FileName = "PCManager_Export.json";

			// get all character names
			for (int i = 0; i < characterItems.Length; i++)
			{
				characterNames[i] = characterItems[i].CharacterName;
			}

			// select characters to export
			DialogWindowSelectStingValueViewModel dataContext = new(characterNames, characterNames.Length);

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
			bool? fileDialogResult = saveFile.ShowDialog();

			if (fileDialogResult.GetValueOrDefault() == false) 
				return;

			string savePath = saveFile.FileName;

			var messageBoxResult = _dialogService.ShowMessage("Single file export? (yes) for 1.json file (no) for individual .json files",
				"single or multiple files", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.Cancel) 
				return;
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
			var characters = new CharacterBase[characterPaths.Length];
			
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

				var character = ReadWriteJsonFile<CharacterBase>.ReadFile(characterPaths[i]) 
					?? throw new Exception("The characater at " + characterPaths[i] + " does not exist.");
				
				characters[i] = character;
			}

			ReadWriteJsonCollection<CharacterBase>.WriteCollection(savePath, characters);
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
					ReadWriteJsonFile<CharacterBase>.WriteFile(savePath + "_" + _characterStore.SelectedCharacter.Name 
						+ ".json", _characterStore.SelectedCharacter);
					savePath += ".json";
					continue;
				}
				var character = ReadWriteJsonFile<CharacterBase>.ReadFile(characterPaths[i]);

				if (character is null)
				{
					throw new Exception("Character path " + characterPaths[i] + " does not exist.");
				}

				ReadWriteJsonFile<CharacterBase>.WriteFile(savePath + "_" + character.Name + ".json", character);
				savePath += ".json";
			}
		}
	}
}
