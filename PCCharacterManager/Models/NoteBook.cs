using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class NoteBook
	{
		public ObservableCollection<NoteSection> NoteSections { get; set; }

		public NoteBook()
		{
			NoteSections = new ObservableCollection<NoteSection>();
		}

		public NoteSection NewNoteSection()
		{
			NoteSections.Add(new NoteSection("New Note"));
			return NoteSections.Last();
		}

		/// <summary>
		/// adds a new note section with a specified title
		/// </summary>
		/// <param name="_noteSectionTitle">title of the new note section</param>
		public void NewNoteSection(NoteSection _noteSectionTitle)
		{
			if (_noteSectionTitle == null) return;

			NoteSections.Add(_noteSectionTitle);
		}

		/// <summary>
		/// Remove a note section from the book
		/// </summary>
		/// <param name="_noteSection">title of the note section to remove</param>
		public void DeleteNoteSection(NoteSection _noteSection)
		{
			if (_noteSection == null) return;

			NoteSections.Remove(_noteSection);
		}

		/// <summary>
		/// creates a new note in the specified section
		/// </summary>
		/// <param name="_sectionTitle">section to add the note to</param>
		public void NewNote(string _sectionTitle)
		{
			foreach (NoteSection noteSection in NoteSections)
			{
				if (noteSection.SectionTitle.ToLower().Equals(_sectionTitle.ToLower()))
				{
					noteSection.Add(new Note());
				}
			}
		}

		/// <summary>
		/// finds the note section with the specified title
		/// </summary>
		/// <param name="_sectionTitle">title of the note section wanted</param>
		/// <returns>the 1st note section with the given title or null if there is no match</returns>
		public NoteSection? GetSection(string _sectionTitle)
		{
			foreach (NoteSection noteSection in NoteSections)
			{
				if (noteSection.SectionTitle.ToLower().Equals(_sectionTitle.ToLower()))
					return noteSection;	
			}

			return null;
		}
	}
}
