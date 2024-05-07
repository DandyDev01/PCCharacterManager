using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
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
	public class AddNoteSectionToNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel viewModel;
		private readonly DialogServiceBase _dialogService;

		public AddNoteSectionToNoteBookCommand(CharacterNoteBookViewModel _viewModel, DialogServiceBase dialogService)
		{
			viewModel = _viewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel();
			window.DataContext = windowVM;

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			string sectionTitle = windowVM.Answer;
			NoteSection newNoteSection = new NoteSection(sectionTitle);
			viewModel.NoteBook!.NewNoteSection(newNoteSection);
			viewModel.NoteSectionsToDisplay.Add(newNoteSection);
		}
	}
}
