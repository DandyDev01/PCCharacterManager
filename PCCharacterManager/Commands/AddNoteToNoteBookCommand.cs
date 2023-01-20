using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using PCCharacterManager.DialogWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace PCCharacterManager.Commands
{
	public class AddNoteToNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel viewModel;

		public AddNoteToNoteBookCommand(CharacterNoteBookViewModel _viewModel)
		{
			viewModel = _viewModel;
		}

		public override void Execute(object parameter)
		{
			// there are no note sections
			if(viewModel.NoteBook.NoteSections.Count == 0)
			{
				MessageBox.Show("You need to create a notes section before creating any notes", 
					"need at least 1 notes section", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			// there is only 1 note section, so you don't need to select one
			if(viewModel.NoteBook.NoteSections.Count == 1)
			{
				viewModel.NoteBook.NoteSections.First().Add(new Note("new Note"));
				return;
			}

			// a note section is selected, add a new note to it
			if(viewModel.SelectedSection != null)
			{
				viewModel.SelectedSection.Add(new Note("new note"));
				return;
			}

			#region select section(s) to add new note to
			string[] sectionTitles = new string[viewModel.NoteBook.NoteSections.Count];
			for (int i = 0; i < sectionTitles.Length; i++)
			{
				sectionTitles[i] = viewModel.NoteBook.NoteSections[i].SectionTitle;
			}

			Window dialogWindow = new SelectStringValueDialogWindow();
			DialogWindowListViewSelectItemViewModel dataContext = new DialogWindowListViewSelectItemViewModel(dialogWindow, sectionTitles, 1);
			dialogWindow.DataContext = dataContext;
			var result = dialogWindow.ShowDialog();

			if (result == false) return;

			string selectedSection = dataContext.SelectedItems.First();

			foreach (NoteSection noteSection in viewModel.NoteBook.NoteSections)
			{
				if (noteSection.SectionTitle.Equals(selectedSection))
				{
					noteSection.Add(new Note("new note"));
				}
			}
			#endregion
		}
	}
}
