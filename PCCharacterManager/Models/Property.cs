using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Property : ObservableObject, ICloneable
	{

		private string _name;
		private string _desc;
		private bool _hidden;

		public string Name
		{
			get { return _name; }
			set { OnPropertyChanged(ref _name, value); }
		}
		public string Desc
		{
			get { return _desc; }
			set { OnPropertyChanged(ref _desc, value); }
		}
		public bool Hidden
		{
			get { return _hidden; }
			set { OnPropertyChanged(ref _hidden, value); }
		}

		public Property(string name, string desc)
		{
			_name = name;
			_desc = desc;
			_hidden = false;
		}

		public Property(string name, string desc, bool hidden)
		{
			_name = name;
			_desc = desc;
			_hidden = hidden;
		}

		public Property()
		{
			_name = string.Empty;
			_desc = string.Empty;
			_hidden = false;
		}

		public object Clone()
		{
			return new Property
			{
				_name = _name,
				_desc = _desc,
				_hidden = _hidden
			};
		}
	}
}
