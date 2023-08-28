using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ItemDisplayVMPool : IPool<ItemDisplayViewModel>
	{
		public ItemDisplayVMPool(int count)
		{
			items = new List<ItemDisplayViewModel>();
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				items.Add(new ItemDisplayViewModel());
				FreeItems++;
			}
		}
	}
}
