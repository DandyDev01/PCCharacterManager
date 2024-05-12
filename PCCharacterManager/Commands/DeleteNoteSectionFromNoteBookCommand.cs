using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class DeleteNoteSectionFromNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel _characterNoteBookViewModel;
		private readonly DialogServiceBase _dialogService;

		public DeleteNoteSectionFromNoteBookCommand(CharacterNoteBookViewModel characterNoteBookViewModel, DialogServiceBase dialogService)
		{
			_characterNoteBookViewModel = characterNoteBookViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_characterNoteBookViewModel.NoteBook is null)
				return;

			NoteBook noteBook = _characterNoteBookViewModel.NoteBook;

			string[] sectionTitles = new string[noteBook.NoteSections.Count];
			for (int i = 0; i < sectionTitles.Length; i++)
			{
				sectionTitles[i] = noteBook.NoteSections[i].SectionTitle;
			}

			DialogWindowListViewSelectItemViewModel dataContext = 
				new(sectionTitles, sectionTitles.Length);

			string results = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowListViewSelectItemViewModel>(dataContext, r =>
			{
				results = r.ToString();
			});

			if (results == false.ToString())
				return;

			string[] selectedSections = dataContext.SelectedItems.ToArray();
			List<NoteSection> sectionsToRemove = noteBook.NoteSections.
				Where(x => selectedSections.Contains(x.SectionTitle)).ToList();
		
			foreach (var item in sectionsToRemove)
			{
				noteBook.NoteSections.Remove(item);
				_characterNoteBookViewModel.NoteSectionsToDisplay.Remove(item);
			}
		}
	}
}
