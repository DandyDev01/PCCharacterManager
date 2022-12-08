using Microsoft.Win32;
using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.IO;
using System.Linq;
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

		private CharacterStore characterStore;
		private ICharacterDataService dataService;

		public ICommand NewCharacterCommand { get; private set; }
		public ICommand DeleteCharacterCommand { get; private set; }
		public ICommand SaveCharactersCommand { get; private set; }
		public ICommand LevelCharacterCommand { get; private set; }
		public ICommand ExportCharacterCommand { get; }
		public ICommand OpenCommand { get; }

		//NOTE: because of the way that CharacterCreateWindow is made the program does
		//		not end when main window is closed. 

		public MainWindowViewModel()
		{
			characterStore = new CharacterStore();
			dataService = new JsonCharacterDataService(characterStore);

			characterStore.SelectedCharacterChange += SaveCharacter;

			tabVM = new TabControlViewModel(characterStore, dataService);
			currView = tabVM;

			NewCharacterCommand = new RelayCommand(CreateCharacterWindow);
			DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
			SaveCharactersCommand = new SaveCharacterCommand(dataService, characterStore);
			LevelCharacterCommand = new LevelCharacterCommand(characterStore);
			ExportCharacterCommand = new CharacterExportCommand(characterStore, tabVM);
			OpenCommand = new OpenCharacterCommand(characterStore);
		}

		private void DeleteCharacter()
		{
			tabVM.CharacterListVM.DeleteCharacter();
		}

		private void CreateCharacterWindow()
		{
			tabVM.CharacterListVM.CreateCharacterWindow();
		}

		private void SaveCharacter(Character c = null)
		{
			dataService.Save(characterStore.SelectedCharacter);
		}
	}
}
