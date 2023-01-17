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
		private string notes;
		public string Notes
		{
			get { return notes; }
			set { OnPropertyChanged(ref notes, value); }
		}

		private string title;
		public string Title
		{
			get { return title; }
			set { OnPropertyChanged(ref title, value); }
		}

		public Note() 
		{
			notes = string.Empty;
			title = string.Empty;
		}

		public Note(string _title)
		{
			title = _title;
			notes = "";
		}

		public Note(string _title, string _desc)
		{
			title = _title;
			notes = _desc;
		}
	}
}
