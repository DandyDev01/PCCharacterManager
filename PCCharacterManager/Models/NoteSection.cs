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
		private string sectionTitle;
		public string SectionTitle
		{
			get { return sectionTitle; }
			set
			{
				OnPropertyChanged(ref sectionTitle, value);
			}
		}

		public ObservableCollection<Note> Notes { get; private set; }

		public NoteSection(string _sectionTitle)
		{
			sectionTitle = _sectionTitle;
			Notes = new ObservableCollection<Note>();
		}

		/// <summary>
		/// adds a given note to the section
		/// </summary>
		/// <param name="_note">note to remove</param>
		public void Add(Note _note)
		{
			if (_note == null) return;

			Notes.Add(_note);
		}

		/// <summary>
		/// removes a given note from the section
		/// </summary>
		/// <param name="_note">note to remove</param>
		public void Remove(Note _note)
		{
			if(_note == null || !Notes.Contains(_note)) return;

			Notes.Remove(_note);
		}
	}
}
