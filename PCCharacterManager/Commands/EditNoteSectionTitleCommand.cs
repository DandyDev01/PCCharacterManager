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
		private readonly CharacterNoteBookViewModel viewModel;
		private readonly DialogServiceBase _dialogService;

		public EditNoteSectionTitleCommand(CharacterNoteBookViewModel _viewModel, DialogServiceBase dialogService)
		{
			viewModel = _viewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (viewModel.SelectedSection == null)
			{
				MessageBox.Show("No section selected", "Requres selected section",
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
			viewModel.SelectedSection.SectionTitle = inputTitle;
		}
	}
}
