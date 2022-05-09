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

		public Property(string _name, string _desc)
		{
			Name = _name;
			Desc = _desc;
		}

		public Property()
		{
			Name = string.Empty;
			Desc = string.Empty;
		}

	}
}
