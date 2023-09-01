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
		protected Item? boundItem;
		public Item? BoundItem
		{
			get { return boundItem; }
			set { OnPropertyChanged(ref boundItem, value); }
		}

		protected bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChanged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		protected string displayName;
		public string DisplayName
		{
			get { return displayName; }
			set
			{
				boundItem.Name = value;
				OnPropertyChanged(ref displayName, value);
			}
		}

		protected string displayDesc;
		public string DisplayDesc
		{
			get { return displayDesc; }
			set
			{
				boundItem.Desc = value;
				OnPropertyChanged(ref displayDesc, value);
			}
		}

		protected int displayQuantity;
		public int DisplayQuantity
		{
			get { return displayQuantity; }
			set
			{
				BoundItem.Quantity = value;
				OnPropertyChanged(ref displayQuantity, value);
			}
		}

		private string displayWeight;
		public string DisplayWeight
		{
			get
			{
				return displayWeight;
			}
			set
			{
				OnPropertyChanged(ref displayWeight, value);
			}
		}

		private string displayCost;
		public string DisplayCost
		{
			get
			{
				return displayCost;
			}
			set
			{
				OnPropertyChanged(ref displayCost, value);
			}
		}

		private ItemType displayItemType;
		public ItemType DisplayItemType
		{
			get
			{
				return displayItemType;
			}
			set
			{
				OnPropertyChanged(ref displayItemType, value);
			}
		}

		private ItemCategory displayItemCategory;
		public ItemCategory DisplayItemCategory
		{
			get
			{
				return displayItemCategory;
			}
			set
			{
				OnPropertyChanged(ref displayItemCategory, value);
			}
		}

		public ItemViewModel(Item _item)
		{
			boundItem = _item;

			displayName = _item.Name;
			displayDesc = _item.Desc;
			displayWeight = _item.Weight;
			displayCost = _item.Cost;
			displayQuantity = _item.Quantity;
			displayItemCategory = _item.Category;
			displayItemType = _item.Type;
		}

		public ItemViewModel()
		{
			displayName = string.Empty;
			displayDesc = string.Empty;
			displayWeight = string.Empty;
			displayCost = string.Empty;
			displayQuantity = 0;
			displayItemCategory = ItemCategory.Item;
			displayItemType = ItemType.Spear;
		}

		public void Bind(Item _item)
		{
			boundItem = _item;

			displayName = _item.Name;
			displayDesc = _item.Desc;
			displayWeight = _item.Weight;
			displayCost = _item.Cost;
			displayQuantity = _item.Quantity;
			displayItemCategory = _item.Category;
			displayItemType = _item.Type;
		}
	}
}
