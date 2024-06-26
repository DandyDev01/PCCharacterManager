﻿using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using PCCharacterManager.DialogWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using PCCharacterManager.Services;

namespace PCCharacterManager.Commands
{
	public class AddNoteToNoteBookCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel _characterNoteBookViewModel;
		private readonly DialogServiceBase _dialogService;

		public AddNoteToNoteBookCommand(CharacterNoteBookViewModel characterNoteBookViewModel, DialogServiceBase dialogService)
		{
			_characterNoteBookViewModel = characterNoteBookViewModel;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			if (_characterNoteBookViewModel.NoteBook is null)
				return;

			if (Validate(_characterNoteBookViewModel.NoteBook) == false)
				return;

			string[] sectionTitles = new string[_characterNoteBookViewModel.NoteBook.NoteSections.Count];
			for (int i = 0; i < sectionTitles.Length; i++)
			{
				sectionTitles[i] = _characterNoteBookViewModel.NoteBook.NoteSections[i].SectionTitle;
			}

			string titleOfSectionToAddNote = GetSectionToAddNote(sectionTitles);

			foreach (NoteSection noteSection in _characterNoteBookViewModel.NoteBook.NoteSections)
			{
				if (noteSection.SectionTitle.Equals(titleOfSectionToAddNote))
				{
					noteSection.Add(new Note("new note"));
				}
			}
		}

		private string GetSectionToAddNote(string[] sectionTitles)
		{
			DialogWindowListViewSelectItemViewModel dataContext = new(sectionTitles, 1);

			string results = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowListViewSelectItemViewModel>(dataContext, r =>
			{
				results = r.ToString();
			});

			if (results == false.ToString())
				return string.Empty;


			return dataContext.SelectedItems.First();
		}

		private bool Validate(NoteBook noteBook)
		{
			// there are no note sections
			if (noteBook.NoteSections.Count == 0)
			{
				_dialogService.ShowMessage("You need to create a notes section before creating any notes",
					"need at least 1 notes section", MessageBoxButton.OK, MessageBoxImage.Information);
				return false;
			}

			// there is only 1 note section, so you don't need to select one
			if (noteBook.NoteSections.Count == 1)
			{
				noteBook.NoteSections.First().Add(new Note("new Note"));
				return false;
			}

			// a note section is selected, add a new note to it
			if (_characterNoteBookViewModel.SelectedSection != null)
			{
				_characterNoteBookViewModel.SelectedSection.Add(new Note("new note"));
				return false;
			}

			return true;
		}
	}
}
