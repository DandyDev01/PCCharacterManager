﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class NoteBook
	{
		public List<Note> Notes { get; set; }

		public NoteBook()
		{
			Notes = new List<Note>();
		}

		public Note NewNote()
		{
			Notes.Add(new Note("New Note"));
			return Notes.Last();
		}

		public void NewNote(Note _note)
		{
			Notes.Add(_note);
		}

		public void DeleteNote(Note note)
		{
			Notes.Remove(note);
		}
	}
}
