using PCCharacterManager.ViewModels;
using System.Collections.Generic;

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
