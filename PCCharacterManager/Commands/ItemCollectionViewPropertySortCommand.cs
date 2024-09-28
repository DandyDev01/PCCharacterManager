using PCCharacterManager.ViewModels;

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
