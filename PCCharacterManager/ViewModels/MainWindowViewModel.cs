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
		private readonly DialogServiceBase _dialogService;
		private readonly RecoveryBase _recovery;

		public ICommand NewCharacterCommand { get; }
		public ICommand DeleteCharacterCommand { get; }
		public ICommand SaveCharactersCommand { get; }
		public ICommand LevelCharacterCommand { get; }
		public ICommand ExportCharacterCommand { get; }
		public ICommand OpenCommand { get; }
		public ICommand EditCharacterCommand { get; }
		public ICommand UndoCommand { get; }
		public ICommand RedoCommand { get; }

		//NOTE: because of the way that CharacterCreateWindow is made the program does
		//		not end when main window is closed. 

		public MainWindowViewModel()
		{
			_characterStore = new CharacterStore();
			_dataService = new JsonCharacterDataService(_characterStore);
			_dialogService = new DialogService();
			_recovery = new SimpleCharacterRecovery();

			_characterStore.SaveSelectedCharacterOnChange += SaveCharacter;

			_tabVM = new TabControlViewModel(_characterStore, _dataService, _dialogService, _recovery);
			_currView = _tabVM;

			NewCharacterCommand = new CreateCharacterCommand(_characterStore, _dialogService);
			DeleteCharacterCommand = new DeleteCharacterCommand(_tabVM.CharacterListVM, _dataService, _characterStore, _dialogService);
			SaveCharactersCommand = new SaveCharacterCommand(this);
			LevelCharacterCommand = new LevelCharacterCommand(_characterStore, _dialogService);
			ExportCharacterCommand = new CharacterExportCommand(_characterStore, _tabVM, _dialogService);
			OpenCommand = new OpenCharacterCommand(_characterStore, _dialogService);
			EditCharacterCommand = new RelayCommand(EditCharacter);
			UndoCommand = new RelayCommand(Undo);
			RedoCommand = new RelayCommand(Redo);
		}

		private void Redo()
		{
			DnD5eCharacter character = _recovery.Redo();

			if (character == null)
				return;

			_characterStore.BindSelectedCharacter(character);
		}

		private void Undo()
		{
			DnD5eCharacter character = _recovery.Undo();

			if (character == null)
				return;

			_characterStore.BindSelectedCharacter(character);
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

			DialogWindowEditCharacterViewModel windowVM = new(_characterStore.SelectedCharacter, _dialogService);

			string result = string.Empty;
			_dialogService.ShowDialog<EditCharacterDialogWindow, DialogWindowEditCharacterViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;
		}
	}
}
