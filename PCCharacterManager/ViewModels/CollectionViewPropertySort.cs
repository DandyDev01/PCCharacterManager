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
		private readonly ICollectionView collectionView;

		public CollectionViewPropertySort(ICollectionView _collectionView)
		{
			collectionView = _collectionView;
		}

		public void Sort(string propertyName)
		{
			ListSortDirection sortDirection = ListSortDirection.Ascending;

			foreach (var sortDescription in collectionView.SortDescriptions)
			{
				if (sortDescription.PropertyName == propertyName && sortDescription.Direction == ListSortDirection.Ascending)
				{
					sortDirection = ListSortDirection.Descending;
					break;
				}
			}

			var sortDescriptor = new SortDescription(propertyName, sortDirection);

			collectionView.SortDescriptions.Clear();
			collectionView.SortDescriptions.Add(sortDescriptor);
		}
	}
}
