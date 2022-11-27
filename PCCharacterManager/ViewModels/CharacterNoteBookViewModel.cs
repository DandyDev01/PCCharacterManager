using PCCharacterManager.Commands;
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
		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref searchTerm, value);
				Search(searchTerm);
			}
		}

		private string highlightTerm;
		public string HighlightTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref highlightTerm, value);
				Search(highlightTerm);
			}
		}

		public ObservableCollection<NoteSection> NoteSectionsToDisplay { get; }
		public ObservableCollection<Note> SearchResults { get; }

		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set { OnPropertyChaged(ref selectedNote, value); }
		}

		private NoteSection? selectedSection;
		public NoteSection? SelectedSection
		{
			get { return selectedSection; }
			set { OnPropertyChaged(ref selectedSection, value); }
		}

		public ICommand AddNoteCommand { get; private set; }
		public ICommand AddNoteSectionCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }
		public ICommand DeleteNoteSectionCommand { get; private set; }
		public ICommand EditSectionTitleCommand { get; private set; }

		public CharacterNoteBookViewModel(CharacterStore _characterStore, ICharacterDataService _dataService) : base(_characterStore, _dataService)
		{
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
			EditSectionTitleCommand = new RelayCommand(EditSectionTitle);
		}

		private void EditSectionTitle()
		{
			if(selectedSection == null)
			{
				MessageBox.Show("No section selected", "Requres selected section", 
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel dataContext = new DialogWindowStringInputViewModel(window);
			window.DataContext = dataContext;

			bool? result = window.ShowDialog();

			if ((bool)!result) return;

			string inputTitle = dataContext.Answer;
			selectedSection.SectionTitle = inputTitle;
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected override void OnCharacterChanged(Character newCharacter)
		{
			base.OnCharacterChanged(newCharacter);

			NoteSectionsToDisplay.Clear();

			foreach (var noteSection in selectedCharacter.NoteManager.NoteSections)
			{
				NoteSectionsToDisplay.Add(noteSection);
			}

			if (selectedCharacter.NoteManager.NoteSections.Count <= 0) return;

			SelectedNote = NoteSectionsToDisplay[0].Notes[0];
		}

		/// <summary>
		/// Finds all notes whose title contains a search term
		/// </summary>
		/// <param name="term">term looking for</param>
		private void Search(string term)
		{
			SearchResults.Clear();

			if (term == String.Empty || string.IsNullOrWhiteSpace(SearchTerm))
			{
				SearchResults.Clear();
			}
			else if(term == "*")
			{
				foreach (var section in selectedCharacter.NoteManager.NoteSections.OrderBy(x => x.SectionTitle))
				{
					foreach (var note in section.Notes)
					{
						SearchResults.Add(note);
					}
				}
			}
			else
			{
				foreach (var section in selectedCharacter.NoteManager.NoteSections.OrderBy(x => x.SectionTitle))
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
