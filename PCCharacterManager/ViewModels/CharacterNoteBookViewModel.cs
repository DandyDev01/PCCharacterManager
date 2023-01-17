﻿using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterNoteBookViewModel : TabItemViewModel
	{
		private NoteBook noteBook;
		public NoteBook NoteBook => noteBook;

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChanged(ref searchTerm, value);
				Search(searchTerm);
			}
		}

		private string highlightTerm;
		public string HighlightTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChanged(ref highlightTerm, value);
				Search(highlightTerm);
			}
		}

		public ObservableCollection<NoteSection> NoteSectionsToDisplay { get; }
		public ObservableCollection<Note> SearchResults { get; }

		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set { OnPropertyChanged(ref selectedNote, value); }
		}

		private NoteSection? selectedSection;
		public NoteSection? SelectedSection
		{
			get { return selectedSection; }
			set { OnPropertyChanged(ref selectedSection, value); }
		}

		public ICommand AddNoteCommand { get; private set; }
		public ICommand AddNoteSectionCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }
		public ICommand DeleteNoteSectionCommand { get; private set; }
		public ICommand EditSectionTitleCommand { get; private set; }

		public CharacterNoteBookViewModel(CharacterStore _characterStore, ICharacterDataService _dataService, 
			NoteBook _noteBook) : base(_characterStore, _dataService)
		{
			noteBook = _noteBook;

			characterStore.SelectedCharacterChange += OnCharacterChanged;

			NoteSectionsToDisplay = new ObservableCollection<NoteSection>();
			SearchResults = new ObservableCollection<Note>();
			searchTerm = string.Empty;
			highlightTerm = string.Empty;
			selectedNote = new Note();

			AddNoteCommand = new AddNoteToNoteBookCommand(this);
			AddNoteSectionCommand = new AddNoteSectionToNoteBookCommand(this);
			DeleteNoteCommand = new RemoveNoteFromNoteBookCommand(this);
			DeleteNoteSectionCommand = new DeleteNoteSectionFromNoteBookCommand(this);
			EditSectionTitleCommand = new EditNoteSectionTitleCommand(this);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected override void OnCharacterChanged(Character newCharacter)
		{
			noteBook = newCharacter.NoteManager;

			NoteSectionsToDisplay.Clear();

			foreach (var noteSection in noteBook.NoteSections)
			{
				NoteSectionsToDisplay.Add(noteSection);
			}

			if (noteBook.NoteSections.Count <= 0) return;

			SelectedNote = NoteSectionsToDisplay[0].Notes[0];
		}

		/// <summary>
		/// Finds all notes whose title contains a search term
		/// </summary>
		/// <param name="term">term looking for</param>
		private void Search(string term)
		{
			SearchResults.Clear();

			if (term == string.Empty || string.IsNullOrWhiteSpace(SearchTerm))
			{
				SearchResults.Clear();
			}
			else if(term == "*")
			{
				foreach (var section in noteBook.NoteSections.OrderBy(x => x.SectionTitle))
				{
					foreach (var note in section.Notes)
					{
						SearchResults.Add(note);
					}
				}
			}
			else
			{
				foreach (var section in noteBook.NoteSections.OrderBy(x => x.SectionTitle))
				{
					foreach (var note in section.Notes)
					{
						if (note.Title.ToLower().Contains(term.ToLower()))
						{
							SearchResults.Add(note);
						}
					}
				}
			}
		}
	}
}
