using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class ItemDisplayViewModel : ItemViewModel
	{
		public ItemDisplayViewModel(Item _item)
		{
			boundItem = _item;

			displayName = _item.Name;
			displayDesc = _item.Desc;
			displayQuantity = _item.Quantity;
		}
	}
}
