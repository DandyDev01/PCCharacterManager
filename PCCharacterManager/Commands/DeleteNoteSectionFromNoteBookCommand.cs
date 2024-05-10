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
		private readonly CharacterNoteBookViewModel _viewModel;
		private readonly DialogServiceBase _dialogService;

		public DeleteNoteSectionFromNoteBookCommand(CharacterNoteBookViewModel viewModel, DialogServiceBase dialogService)
		{
			_viewModel = viewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_viewModel.NoteBook is not NoteBook noteBook)
				return;

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
				_viewModel.NoteSectionsToDisplay.Remove(item);
			}
		}
	}
}
