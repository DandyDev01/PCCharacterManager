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
		private readonly CollectionViewPropertySort _collectionView;
		private readonly string _propertyName;

		public ItemCollectionViewPropertySortCommand(CollectionViewPropertySort collectionView, string propertyName)
		{
			_collectionView = collectionView;
			_propertyName = propertyName;
		}

		public override void Execute(object? parameter)
		{
			_collectionView.Sort(_propertyName);
		}
	}
}
