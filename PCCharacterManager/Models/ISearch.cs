using PCCharacterManager.Utility;

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
