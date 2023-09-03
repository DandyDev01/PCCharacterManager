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
			set { OnPropertyChanged(ref currView, value); }
		}

		private TabControlViewModel tabVM;
		public TabControlViewModel TabVM
		{
			get { return tabVM; }
			set { OnPropertyChanged(ref tabVM, value); }
		}

		private readonly CharacterStore characterStore;
		private readonly ICharacterDataService dataService;

		public ICommand NewCharacterCommand { get; }
		public ICommand DeleteCharacterCommand { get; }
		public ICommand SaveCharactersCommand { get; }
		public ICommand LevelCharacterCommand { get; }
		public ICommand ExportCharacterCommand { get; }
		public ICommand OpenCommand { get; }
		public ICommand EditCharacterCommand { get; }

		//NOTE: because of the way that CharacterCreateWindow is made the program does
		//		not end when main window is closed. 

		public MainWindowViewModel()
		{
			characterStore = new CharacterStore();
			dataService = new JsonCharacterDataService(characterStore);

			characterStore.SelectedCharacterChange += SaveCharacter;

			tabVM = new TabControlViewModel(characterStore, dataService);
			currView = tabVM;

			NewCharacterCommand = new CreateCharacterCommand(characterStore);
			DeleteCharacterCommand = new DeleteCharacterCommand(tabVM.CharacterListVM, dataService, characterStore);
			SaveCharactersCommand = new SaveCharacterCommand(this);
			LevelCharacterCommand = new LevelCharacterCommand(characterStore);
			ExportCharacterCommand = new CharacterExportCommand(characterStore, tabVM);
			OpenCommand = new OpenCharacterCommand(characterStore);
			EditCharacterCommand = new RelayCommand(EditCharacter);
		}

		private void SaveCharacter(DnD5eCharacter? character = null)
		{
			if (tabVM == null) 
				return;

			if (character == null)
				return;

			tabVM.CharacterListVM.SaveCharacter();
		}

		private void EditCharacter()
		{
			if (characterStore.SelectedCharacter == null)
				return;

			Window window = new EditCharacterDialogWindow();
			DialogWindowEditCharacterViewModel windowVM = new(window, characterStore.SelectedCharacter);
			window.DataContext = windowVM;

			window.ShowDialog();
		}
	}
}
