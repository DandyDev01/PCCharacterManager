using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
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

		public AddNoteSectionToNoteBookCommand(CharacterNoteBookViewModel _viewModel)
		{
			viewModel = _viewModel;
		}

		public override void Execute(object? parameter)
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window);
			window.DataContext = windowVM;

			var result = window.ShowDialog();

			if (result == false) return;

			string sectionTitle = windowVM.Answer;
			NoteSection newNoteSection = new NoteSection(sectionTitle);
			viewModel.NoteBook!.NewNoteSection(newNoteSection);
			viewModel.NoteSectionsToDisplay.Add(newNoteSection);
		}
	}
}
