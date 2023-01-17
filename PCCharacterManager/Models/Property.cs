﻿using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Property : ObservableObject, ICloneable
	{

		private string name;
		private string desc;
		private bool hidden;

		public string Name
		{
			get { return name; }
			set { OnPropertyChanged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChanged(ref desc, value); }
		}
		public bool Hidden
		{
			get { return hidden; }
			set { OnPropertyChanged(ref hidden, value); }
		}

		public Property(string _name, string _desc)
		{
			name = _name;
			desc = _desc;
			hidden = false;
		}

		public Property(string _name, string _desc, bool _hidden)
		{
			name = _name;
			desc = _desc;
			hidden = _hidden;
		}

		public Property()
		{
			name = string.Empty;
			desc = string.Empty;
			hidden = false;
		}

		public object Clone()
		{
			return new Property
			{
				name = name,
				desc = desc,
				hidden = hidden
			};
		}
	}
}
