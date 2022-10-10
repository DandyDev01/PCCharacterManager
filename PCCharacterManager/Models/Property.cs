using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Property : ObservableObject
	{

		private string name;
		private string desc;
		private bool hidden;

		public string Name
		{
			get { return name; }
			set { OnPropertyChaged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChaged(ref desc, value); }
		}
		public bool Hidden
		{
			get { return hidden; }
			set { OnPropertyChaged(ref hidden, value); }
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

	}
}
