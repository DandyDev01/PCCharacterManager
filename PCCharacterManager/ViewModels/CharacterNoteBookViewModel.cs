using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
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

		private List<Property> raceFeatures;
		public List<Property> RaceFeatures
		{
			get { return raceFeatures; }
			set
			{
				OnPropertyChaged(ref raceFeatures, value);
			}
		}

		private List<Note> notes;
		public List<Note> Notes
		{
			get { return notes; }
			set { OnPropertyChaged(ref notes, value); }
		}

		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set { OnPropertyChaged(ref selectedNote, value); }
		}

		public ICommand AddNoteCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }

		public CharacterNoteBookViewModel(CharacterStore _characterStore, ICharacterDataService _dataService, Character _selectedCharacter = null) : base(_characterStore, _dataService, _selectedCharacter)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;
			raceFeatures = new List<Property>();

			AddNoteCommand = new RelayCommand(AddNote);
			DeleteNoteCommand = new RelayCommand(DeleteNote);
			notes = new List<Note>();
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			base.OnCharacterChanged(newCharacter);

			raceFeatures.Clear();
			List<Property> temp = new List<Property>();
			temp.AddRange(newCharacter.Race.Features);
			temp.AddRange(newCharacter.Race.RaceVariant.Properties);
			RaceFeatures = new List<Property>(temp);

			Notes = new List<Note>();
			Notes.AddRange(selectedCharacter.NoteManager.Notes);
			SelectedNote = Notes[0];
		}

		private void Search(string term)
		{
			List<Note> found = new List<Note>();

			if (term == String.Empty || string.IsNullOrWhiteSpace(SearchTerm))
				found.AddRange(selectedCharacter.NoteManager.Notes);
			else
			{
				foreach (var note in selectedCharacter.NoteManager.Notes)
				{
					if (note.Title.ToLower().Contains(term.ToLower()))
					{
						found.Add(note);
					}
				}
			}

			Notes = new List<Note>();
			Notes.AddRange(found);
		}
		private void AddNote()
		{
			selectedCharacter.NoteManager.NewNote();

			Notes = new List<Note>();
			Notes.AddRange(selectedCharacter.NoteManager.Notes);
		}
		private void DeleteNote()
		{
			var result = MessageBox.Show("Are you sure you want to delete " +
				 selectedNote.Title + "?", "Permenently Delete Note",
				 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
				return;


			selectedCharacter.NoteManager.DeleteNote(selectedNote);

			Notes = new List<Note>();
			Notes.AddRange(selectedCharacter.NoteManager.Notes);
		}
	}
}
