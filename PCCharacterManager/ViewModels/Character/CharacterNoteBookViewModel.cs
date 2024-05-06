using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterNoteBookViewModel : ObservableObject
	{
		private NoteBook _noteBook;
		public NoteBook NoteBook => _noteBook;

		public ObservableCollection<NoteSection> NoteSectionsToDisplay { get; }
		public ObservableCollection<Note> SearchResults { get; }

		private Note _selectedNote;
		public Note SelectedNote
		{
			get { return _selectedNote; }
			set 
			{
				OnPropertyChanged(ref _selectedNote, value);
				selectedNoteChange?.Invoke(value);
			}
		}

		private NoteSection? selectedSection;
		public NoteSection? SelectedSection
		{
			get { return selectedSection; }
			set { OnPropertyChanged(ref selectedSection, value); }
		}
		
		private string _searchTerm;
		public string SearchTerm
		{
			get { return _searchTerm; }
			set
			{
				OnPropertyChanged(ref _searchTerm, value);
				Search(_searchTerm);
			}
		}

		private string _highlightTerm;
		public string HighlightTerm
		{
			get
			{
				return _highlightTerm;
			}
			set
			{
				OnPropertyChanged(ref _highlightTerm, value);
			}
		}

		public ICommand AddNoteCommand { get; }
		public ICommand AddNoteSectionCommand { get; }
		public ICommand DeleteNoteCommand { get; }
		public ICommand DeleteNoteSectionCommand { get; }
		public ICommand EditSectionTitleCommand { get; }
		public ICommand FindInNoteCommand { get; }

		// for talking to the view about the richtextbox.document
		public Action<Note>? selectedNoteChange;
		public Action? characterChange;

		public CharacterNoteBookViewModel(CharacterStore characterStore, DialogService dialogService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_noteBook = characterStore.SelectedCharacter.NoteManager;

			NoteSectionsToDisplay = new ObservableCollection<NoteSection>();
			SearchResults = new ObservableCollection<Note>();
			_searchTerm = string.Empty;
			_highlightTerm = string.Empty;
			_selectedNote = new Note();

			AddNoteCommand = new AddNoteToNoteBookCommand(this);
			AddNoteSectionCommand = new AddNoteSectionToNoteBookCommand(this, dialogService);
			DeleteNoteCommand = new RemoveNoteFromNoteBookCommand(this);
			DeleteNoteSectionCommand = new DeleteNoteSectionFromNoteBookCommand(this);
			EditSectionTitleCommand = new EditNoteSectionTitleCommand(this, dialogService);
			FindInNoteCommand = new RelayCommand(FindInNote);
		}

		private void FindInNote()
		{
			if (_selectedNote == null) return;
			if(!_selectedNote.Notes.Contains(_highlightTerm)) return;

			// open small window in corner area.
			// window cannot be moved
			// window will have a textbox for the term to look for
			// highlight the term in the note textbox
			// show a count for the term
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			_noteBook = newCharacter.NoteManager;

			NoteSectionsToDisplay.Clear();

			foreach (var noteSection in _noteBook.NoteSections)
			{
				NoteSectionsToDisplay.Add(noteSection);
			}

			if (_noteBook.NoteSections.Count <= 0) return;

			SelectedNote = NoteSectionsToDisplay[0].Notes[0];

			characterChange?.Invoke();
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
				foreach (var section in _noteBook.NoteSections.OrderBy(x => x.SectionTitle))
				{
					foreach (var note in section.Notes)
					{
						SearchResults.Add(note);
					}
				}
			}
			else
			{
				foreach (var section in _noteBook.NoteSections.OrderBy(x => x.SectionTitle))
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
