using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class ItemDisplayViewModel : ObservableObject
	{
		private Item boundItem;
		public Item BoundItem
		{
			get { return boundItem; }
			set
			{
				OnPropertyChaged(ref boundItem, value);
			}
		}

		private string itemName;
		public string ItemName
		{
			get { return itemName; }
			set
			{
				boundItem.Name = value;
				OnPropertyChaged(ref itemName, value);
			}
		}

		private string itemDesc;
		public string ItemDesc
		{
			get { return itemDesc; }
			set
			{
				boundItem.Desc = value;
				OnPropertyChaged(ref itemDesc, value);
			}
		}

		private int itemQuantity;
		public int ItemQuantity
		{
			get { return itemQuantity; }
			set
			{
				boundItem.Quantity = value;
				OnPropertyChaged(ref itemQuantity, value);
			}
		}

		public ItemDisplayViewModel(Item _item)
		{
			boundItem = _item;

			itemName = _item.Name;
			itemDesc = _item.Desc;
			itemQuantity = _item.Quantity;
		}
	}
}
