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

		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set { OnPropertyChaged(ref selectedNote, value); }
		}

		public ICommand AddNoteCommand { get; private set; }
		public ICommand AddNoteSectionCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }
		public ICommand DeleteNoteSectionCommand { get; private set; }

		public CharacterNoteBookViewModel(CharacterStore _characterStore, ICharacterDataService _dataService) : base(_characterStore, _dataService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			NoteSectionsToDisplay = new ObservableCollection<NoteSection>();
			searchTerm = string.Empty;
			highlightTerm = string.Empty;
			selectedNote = new Note();

			AddNoteCommand = new AddNoteToNoteBookCommand(this);
			AddNoteSectionCommand = new RelayCommand(AddNoteSection);
			DeleteNoteCommand = new RemoveNoteFromNoteBookCommand(this);
			DeleteNoteSectionCommand = new RelayCommand(DeleteNoteSection);
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

			if (NoteSectionsToDisplay.Count < 0) return;

			SelectedNote = NoteSectionsToDisplay[0].Notes[0];
		}

		private void AddNoteSection()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window);
			window.DataContext = windowVM;

			var result = window.ShowDialog();

			if (result == false) return;

			string sectionTitle = windowVM.Answer;
			NoteSection newNoteSection = new NoteSection(sectionTitle);
			selectedCharacter.NoteManager.NewNoteSection(newNoteSection);
			NoteSectionsToDisplay.Add(newNoteSection);
		}

		private void DeleteNoteSection()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Finds all notes whose title contains a search term
		/// </summary>
		/// <param name="term">term looking for</param>
		private void Search(string term)
		{
			//NoteSectionsToDisplay.Clear();

			//if (term == String.Empty || string.IsNullOrWhiteSpace(SearchTerm))
			//{
			//	foreach (var note in selectedCharacter.NoteManager.NoteSections)
			//	{
			//		NoteSectionsToDisplay.Add(note);
			//	}
			//}
			//else
			//{
			//	foreach (var note in selectedCharacter.NoteManager.NoteSections.OrderBy(x => x.Title))
			//	{
			//		if (note.Title.ToLower().Contains(term.ToLower()))
			//		{
			//			NoteSectionsToDisplay.Add(note);
			//		}
			//	}
			//}
		}
	}
}
