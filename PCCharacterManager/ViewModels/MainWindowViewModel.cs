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
		private Object _currView;
		public Object CurrView
		{
			get { return _currView; }
			set { OnPropertyChanged(ref _currView, value); }
		}

		private TabControlViewModel _tabVM;
		public TabControlViewModel TabVM
		{
			get { return _tabVM; }
			set { OnPropertyChanged(ref _tabVM, value); }
		}

		private readonly CharacterStore _characterStore;
		private readonly ICharacterDataService _dataService;

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
			_characterStore = new CharacterStore();
			_dataService = new JsonCharacterDataService(_characterStore);

			_characterStore.SelectedCharacterChange += SaveCharacter;

			_tabVM = new TabControlViewModel(_characterStore, _dataService);
			_currView = _tabVM;

			NewCharacterCommand = new CreateCharacterCommand(_characterStore);
			DeleteCharacterCommand = new DeleteCharacterCommand(_tabVM.CharacterListVM, _dataService, _characterStore);
			SaveCharactersCommand = new SaveCharacterCommand(this);
			LevelCharacterCommand = new LevelCharacterCommand(_characterStore);
			ExportCharacterCommand = new CharacterExportCommand(_characterStore, _tabVM);
			OpenCommand = new OpenCharacterCommand(_characterStore);
			EditCharacterCommand = new RelayCommand(EditCharacter);
		}

		private void SaveCharacter(DnD5eCharacter? character = null)
		{
			if (_tabVM == null) 
				return;

			if (character == null)
				return;

			_tabVM.CharacterListVM.SaveCharacter();
		}

		private void EditCharacter()
		{
			if (_characterStore.SelectedCharacter == null)
				return;

			Window window = new EditCharacterDialogWindow();
			DialogWindowEditCharacterViewModel windowVM = new(window, _characterStore.SelectedCharacter);
			window.DataContext = windowVM;

			window.ShowDialog();
		}
	}
}
