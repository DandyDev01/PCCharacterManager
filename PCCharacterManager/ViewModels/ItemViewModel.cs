using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class ItemViewModel : ObservableObject
	{
		protected Item _boundItem;
		public Item BoundItem
		{
			get { return _boundItem; }
			set { OnPropertyChanged(ref _boundItem, value); }
		}

		protected bool _isEditMode;
		public bool IsEditMode
		{
			get { return _isEditMode; }
			set
			{
				OnPropertyChanged(ref _isEditMode, value);
				OnPropertyChanged(nameof(IsDisplayMode));
			}
		}

		public bool IsDisplayMode
		{
			get { return !_isEditMode; }
		}

		protected string _displayName;
		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				_boundItem.Name = value;
				OnPropertyChanged(ref _displayName, value);
			}
		}

		protected string _displayDesc;
		public string DisplayDesc
		{
			get { return _displayDesc; }
			set
			{
				_boundItem.Desc = value;
				OnPropertyChanged(ref _displayDesc, value);
			}
		}

		protected int _displayQuantity;
		public int DisplayQuantity
		{
			get { return _displayQuantity; }
			set
			{
				BoundItem.Quantity = value;
				OnPropertyChanged(ref _displayQuantity, value);
			}
		}

		private string _displayWeight;
		public string DisplayWeight
		{
			get
			{
				return _displayWeight;
			}
			set
			{
				_boundItem.Weight = value;
				OnPropertyChanged(ref _displayWeight, value);
			}
		}

		private string _displayCost;
		public string DisplayCost
		{
			get
			{
				return _displayCost;
			}
			set
			{
				OnPropertyChanged(ref _displayCost, value);
			}
		}

		private ItemType _displayItemType;
		public ItemType DisplayItemType
		{
			get
			{
				return _displayItemType;
			}
			set
			{
				OnPropertyChanged(ref _displayItemType, value);
				_boundItem.Type = value;
			}
		}

		private ItemCategory _displayItemCategory;
		public ItemCategory DisplayItemCategory
		{
			get
			{
				return _displayItemCategory;
			}
			set
			{
				OnPropertyChanged(ref _displayItemCategory, value);
				_boundItem.Category = value;
			}
		}

		public ItemViewModel(Item item)
		{
			_boundItem = item;

			_displayName = item.Name;
			_displayDesc = item.Desc;
			_displayWeight = item.Weight;
			_displayCost = item.Cost;
			_displayQuantity = item.Quantity;
			_displayItemCategory = item.Category;
			_displayItemType = item.Type;
		}

		public ItemViewModel()
		{
			_displayName = string.Empty;
			_displayDesc = string.Empty;
			_displayWeight = string.Empty;
			_displayCost = string.Empty;
			_displayQuantity = 0;
			_displayItemCategory = ItemCategory.Item;
			_displayItemType = ItemType.Spear;
			_boundItem = new();
		}

		public void Bind(Item item)
		{
			_boundItem = item;

			_displayName = item.Name;
			_displayDesc = item.Desc;
			_displayWeight = item.Weight;
			_displayCost = item.Cost;
			_displayQuantity = item.Quantity;
			_displayItemCategory = item.Category;
			_displayItemType = item.Type;
		}
	}
}
