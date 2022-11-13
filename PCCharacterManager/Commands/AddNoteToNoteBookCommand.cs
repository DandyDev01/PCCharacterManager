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
			//NoteSection newNote = viewModel.SelectedCharacter.NoteManager.NewNoteSection();
			//newNote.Add(new Note());
			//viewModel.NoteSectionsToDisplay.Add(newNote);
			//viewModel.SelectedNote = newNote.Notes[0];

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