using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public abstract class ISearch<T> : ObservableObject
	{
		protected string _searchTerm = string.Empty;
		public string SearchTerm
		{
			get
			{
				return _searchTerm;
			}
			set
			{
				OnPropertyChanged(ref _searchTerm, value);
			}
		}

		public abstract bool Search(object obj);
	}
}
