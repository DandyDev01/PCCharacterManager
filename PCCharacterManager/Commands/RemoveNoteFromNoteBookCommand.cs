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
	public class RemoveNoteFromNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel _characterNoteBookViewModel;
		private readonly DialogServiceBase _dialogService;

		public RemoveNoteFromNoteBookCommand(CharacterNoteBookViewModel characterNoteBookViewModel, DialogServiceBase dialogService)
		{
			_characterNoteBookViewModel = characterNoteBookViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_characterNoteBookViewModel.SelectedNote == null) return;

			var result = _dialogService.ShowMessage("Are you sure you want to delete " +
				 _characterNoteBookViewModel.SelectedNote.Title + "?", "Permenently Delete Note",
				 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
				return;

			foreach (NoteSection noteSection in _characterNoteBookViewModel.NoteBook.NoteSections)
			{
				if (noteSection.Notes.Contains(_characterNoteBookViewModel.SelectedNote))
				{
					noteSection.Remove(_characterNoteBookViewModel.SelectedNote);
					break;
				}
			}
		}
	}
}
