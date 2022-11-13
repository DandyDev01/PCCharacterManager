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
			if(viewModel.SelectedCharacter.NoteManager.NoteSections.Count == 0)
			{
				MessageBox.Show("You need to create a notes section before creating any notes", 
					"need at least 1 notes section", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			if(viewModel.SelectedCharacter.NoteManager.NoteSections.Count == 1)
			{
				viewModel.SelectedCharacter.NoteManager.NoteSections.First().Add(new Note("new Note"));
				return;
			}

			string[] sectionTitles = new string[viewModel.SelectedCharacter.NoteManager.NoteSections.Count];
			for (int i = 0; i < sectionTitles.Length; i++)
			{
				sectionTitles[i] = viewModel.SelectedCharacter.NoteManager.NoteSections[i].SectionTitle;
			}

			Window dialogWindow = new SelectStringValueDialogWindow();
			DialogWindowListViewSelectItemViewModel dataContext = new DialogWindowListViewSelectItemViewModel(dialogWindow, sectionTitles, 1);
			dialogWindow.DataContext = dataContext;
			var result = dialogWindow.ShowDialog();

			if (result == false) return;

			string selectedSection = dataContext.SelectedItems.First();

			foreach (NoteSection noteSection in viewModel.SelectedCharacter.NoteManager.NoteSections)
			{
				if (noteSection.SectionTitle.Equals(selectedSection))
				{
					noteSection.Add(new Note("new note"));
				}
			}
		}
	}
}
