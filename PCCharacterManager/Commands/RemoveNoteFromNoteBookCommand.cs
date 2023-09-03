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
	public class RemoveNoteFromNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel viewModel;

		public RemoveNoteFromNoteBookCommand(CharacterNoteBookViewModel _viewModel)
		{
			viewModel = _viewModel;
		}

		public override void Execute(object? parameter)
		{
			if (viewModel.SelectedNote == null) return;

			var result = MessageBox.Show("Are you sure you want to delete " +
				 viewModel.SelectedNote.Title + "?", "Permenently Delete Note",
				 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
				return;

			foreach (NoteSection noteSection in viewModel.NoteBook.NoteSections)
			{
				if (noteSection.Notes.Contains(viewModel.SelectedNote))
				{
					noteSection.Remove(viewModel.SelectedNote);
					break;
				}
			}
		}
	}
}
