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
				OnPropertyChaged(ref sectionTitle, value);
			}
		}

		public ObservableCollection<Note> Notes { get; private set; }

		public NoteSection(string _sectionTitle)
		{
			sectionTitle = _sectionTitle;
			Notes = new ObservableCollection<Note>();
		}

		public void Add(Note _note)
		{
			if (_note == null) return;

			Notes.Add(_note);
		}

		public void Remove(Note _note)
		{
			if(_note == null || !Notes.Contains(_note)) return;

			Notes.Remove(_note);
		}
	}
}
