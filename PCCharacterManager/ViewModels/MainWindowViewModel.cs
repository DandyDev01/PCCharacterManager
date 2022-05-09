﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class MainWindowViewModel : ObservableObject
	{
		private Object currView;
		public Object CurrView
		{
			get { return currView; }
			set { OnPropertyChaged(ref currView, value); }
		}

		private TabControlViewModel tabVM;
		public TabControlViewModel TabVM
		{
			get { return tabVM; }
			set { OnPropertyChaged(ref tabVM, value); }
		}

		CharacterStore characterStore;
		ICharacterDataService dataService;

		public ICommand NewCharacterCommand { get; private set; }
		public ICommand SaveCharactersCommand { get; private set; }
		public ICommand LevelCharacterCommand { get; private set; }
		public ICommand LongRestCommand { get; private set; }

		//NOTE: because of the way that CharacterCreateWindow is made the program does
		//		not end when main window is closed. 

		public MainWindowViewModel()
		{

			characterStore = new CharacterStore();
			dataService = new JsonCharacterDataService(characterStore);

			tabVM = new TabControlViewModel(characterStore, dataService);

			currView = tabVM;


			NewCharacterCommand = new RelayCommand(CreateCharacterWindow);

			SaveCharactersCommand = new RelayCommand(SaveCharacters);

			LevelCharacterCommand = new RelayCommand(LevelCharacter);

			LongRestCommand = new RelayCommand(LongRest);
		}

		private void LongRest()
		{

			// regain lost hit points
			// regain spent HitDice up to a number of dice == to half the character's total # of them
			// ask if you want to reset prepared spells
		}

		private void LevelCharacter()
		{
			CharacterLeveler leveler = new CharacterLeveler();

			leveler.LevelCharacter(characterStore.SelectedCharacter);

			OnPropertyChaged("characterStore.SelectedCharacter");
		}

		private void CreateCharacterWindow()
		{
			Window newCharacterWindow = new CreateCharacterDialogWindow();
			newCharacterWindow.DataContext = new DialogWindowCharacterCreaterViewModel(dataService, characterStore, newCharacterWindow);

			newCharacterWindow.ShowDialog();
		}

		private void SaveCharacters()
		{
			List<Character> characters = CharacterListViewModel.Characters.ToList();
			// will not work
			dataService.Save(characters);
		}
	}
}
