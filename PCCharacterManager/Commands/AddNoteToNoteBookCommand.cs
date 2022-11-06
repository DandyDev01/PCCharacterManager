using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Note newNote = viewModel.SelectedCharacter.NoteManager.NewNote();
			viewModel.NotesToDisplay.Add(newNote);
			viewModel.SelectedNote = newNote;
		}
	}
}
