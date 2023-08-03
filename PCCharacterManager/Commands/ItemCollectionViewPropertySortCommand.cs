using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class ItemCollectionViewPropertySortCommand : BaseCommand
	{
		private readonly CollectionViewPropertySort collectionView;
		private readonly string propertyName;

		public ItemCollectionViewPropertySortCommand(CollectionViewPropertySort _collectionView, string _propertyName)
		{
			collectionView = _collectionView;
			propertyName = _propertyName;
		}

		public override void Execute(object parameter)
		{
			collectionView.Sort(propertyName);
		}
	}
}
