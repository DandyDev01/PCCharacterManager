using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ItemVMPool : IPool<ItemViewModel>
	{
		public ItemVMPool(int count)
		{
			_items = new List<ItemViewModel>();
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_items.Add(new ItemViewModel());
				FreeItems++;
			}
		}
	}
}
