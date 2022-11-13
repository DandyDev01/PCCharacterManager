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
	public class DeleteNoteSectionFromNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel viewModel;

		public DeleteNoteSectionFromNoteBookCommand(CharacterNoteBookViewModel _viewModel)
		{
			viewModel = _viewModel;
		}

		public override void Execute(object parameter)
		{
			string[] sectionTitles = new string[viewModel.SelectedCharacter.NoteManager.NoteSections.Count];
			for (int i = 0; i < sectionTitles.Length; i++)
			{
				sectionTitles[i] = viewModel.SelectedCharacter.NoteManager.NoteSections[i].SectionTitle;
			}

			Window dialogWindow = new SelectStringValueDialogWindow();
			DialogWindowListViewSelectItemViewModel dataContext = new DialogWindowListViewSelectItemViewModel(dialogWindow, sectionTitles, sectionTitles.Length);
			dialogWindow.DataContext = dataContext;
			var result = dialogWindow.ShowDialog();

			if (result == false) return;

			string[] selectedSections = dataContext.SelectedItems.ToArray();

			for (int i = 0; i < selectedSections.Length; i++)
			{
				if (viewModel.SelectedCharacter.NoteManager.NoteSections[i].SectionTitle.Equals(selectedSections[i]))
				{
					//NoteSection noteSectionToRemove = viewModel.SelectedCharacter.NoteManager.NoteSections[i];
					viewModel.SelectedCharacter.NoteManager.NoteSections.RemoveAt(i);
					viewModel.NoteSectionsToDisplay.RemoveAt(i);
				}
			}
		}
	}
}
