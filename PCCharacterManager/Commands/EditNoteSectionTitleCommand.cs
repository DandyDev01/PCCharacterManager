using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class EditNoteSectionTitleCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel _characterNoteBookViewModel;
		private readonly DialogServiceBase _dialogService;

		public EditNoteSectionTitleCommand(CharacterNoteBookViewModel characterNoteBookViewModel, DialogServiceBase dialogService)
		{
			_characterNoteBookViewModel = characterNoteBookViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_characterNoteBookViewModel.SelectedSection == null)
			{
				_dialogService.ShowMessage("No section selected", "Requres selected section",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			DialogWindowStringInputViewModel dataContext = new();

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			string inputTitle = dataContext.Answer;
			_characterNoteBookViewModel.SelectedSection.SectionTitle = inputTitle;
		}
	}
}
