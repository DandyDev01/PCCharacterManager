using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class CollectionViewPropertySort
	{
		private readonly ICollectionView _collectionView;

		public CollectionViewPropertySort(ICollectionView collectionView)
		{
			_collectionView = collectionView;
		}

		public void Sort(string propertyName)
		{
			ListSortDirection sortDirection = ListSortDirection.Ascending;

			foreach (var sortDescription in _collectionView.SortDescriptions)
			{
				if (sortDescription.PropertyName == propertyName && sortDescription.Direction == ListSortDirection.Ascending)
				{
					sortDirection = ListSortDirection.Descending;
					break;
				}
			}

			var sortDescriptor = new SortDescription(propertyName, sortDirection);

			_collectionView.SortDescriptions.Clear();
			_collectionView.SortDescriptions.Add(sortDescriptor);
		}
	}
}
