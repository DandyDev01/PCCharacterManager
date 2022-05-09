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

		public ObservableCollection<Note> NotesToDisplay { get; }

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

			NotesToDisplay = new ObservableCollection<Note>();

			AddNoteCommand = new RelayCommand(AddNote);
			DeleteNoteCommand = new RelayCommand(DeleteNote);
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			base.OnCharacterChanged(newCharacter);

			NotesToDisplay.Clear();

			foreach (var note in selectedCharacter.NoteManager.Notes)
			{
				NotesToDisplay.Add(note);
			}

			SelectedNote = NotesToDisplay[0];
		}

		private void Search(string term)
		{
			NotesToDisplay.Clear();

			if (term == String.Empty || string.IsNullOrWhiteSpace(SearchTerm))
			{
				foreach (var note in selectedCharacter.NoteManager.Notes)
				{
					NotesToDisplay.Add(note);
				}
			}
			else
			{
				foreach (var note in selectedCharacter.NoteManager.Notes.OrderBy(x => x.Title))
				{
					if (note.Title.ToLower().Contains(term.ToLower()))
					{
						NotesToDisplay.Add(note);
					}
				}
			}
		}

		private void AddNote()
		{
			selectedCharacter.NoteManager.NewNote();

			NotesToDisplay.Add(selectedCharacter.NoteManager.Notes.Last());
		}
		private void DeleteNote()
		{
			var result = MessageBox.Show("Are you sure you want to delete " +
				 selectedNote.Title + "?", "Permenently Delete Note",
				 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
				return;

			NotesToDisplay.Remove(selectedNote);
			selectedCharacter.NoteManager.DeleteNote(selectedNote);
		}
	}
}
