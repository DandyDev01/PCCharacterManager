using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class NoteSection : ObservableObject
	{
		private string _sectionTitle;
		public string SectionTitle
		{
			get { return _sectionTitle; }
			set
			{
				OnPropertyChanged(ref _sectionTitle, value);
			}
		}

		public ObservableCollection<Note> Notes { get; private set; }

		public NoteSection(string sectionTitle)
		{
			_sectionTitle = sectionTitle;
			Notes = new ObservableCollection<Note>();
		}

		/// <summary>
		/// adds a given note to the section
		/// </summary>
		/// <param name="note">note to remove</param>
		public void Add(Note note)
		{
			if (note == null) 
				return;

			Notes.Add(note);
		}

		/// <summary>
		/// removes a given note from the section
		/// </summary>
		/// <param name="note">note to remove</param>
		public void Remove(Note note)
		{
			if(note == null || Notes.Contains(note) == false) 
				return;

			Notes.Remove(note);
		}
	}
}
