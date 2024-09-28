using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;

namespace PCCharacterManager.Commands
{
	public class AddNoteSectionToNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel _characterNoteBookViewModel;
		private readonly DialogServiceBase _dialogService;

		public AddNoteSectionToNoteBookCommand(CharacterNoteBookViewModel characterNoteBookViewModel, DialogServiceBase dialogService)
		{
			_characterNoteBookViewModel = characterNoteBookViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			DialogWindowStringInputViewModel dataContext = new();

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			string sectionTitle = dataContext.Answer;
			NoteSection newNoteSection = new(sectionTitle);
			_characterNoteBookViewModel.NoteBook!.NewNoteSection(newNoteSection);
			_characterNoteBookViewModel.NoteSectionsToDisplay.Add(newNoteSection);
		}
	}
}
