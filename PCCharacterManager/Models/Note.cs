using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Note : ObservableObject
	{
		private string _notes;
		public string Notes
		{
			get { return _notes; }
			set{ OnPropertyChanged(ref _notes, value); }
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { OnPropertyChanged(ref _title, value); }
		}

		public Note() 
		{
			_notes = string.Empty;
			_title = string.Empty;
		}

		public Note(string title)
		{
			_title = title;
			_notes = "";
		}

		public Note(string title, string desc)
		{
			_title = title;
			_notes = desc;
		}
	}
}
