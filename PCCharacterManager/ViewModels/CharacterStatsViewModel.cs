using PCCharacterManager.DialogWindows;
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
	public class CharacterStatsViewModel : TabItemViewModel
	{
		public ICommand EditCharacterCommand { get; private set; }

		public CharacterStatsViewModel(CharacterStore _characterStore, ICharacterDataService dataService)
			: base(_characterStore, dataService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;
			EditCharacterCommand = new RelayCommand(EditCharacter);
		}

		private void EditCharacter()
		{ 
			Window window = new EditCharacterDialogWindow();
			DialogWindowEditCharacterViewModel windowVM = new DialogWindowEditCharacterViewModel(window, SelectedCharacter);
			window.DataContext = windowVM;

			window.ShowDialog();
		}
	}
}
