using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ItemEditableVMPool : IPool<ItemEditableViewModel>
	{
		private readonly PropertyEditableVMPool propertyVMPool;

		public ItemEditableVMPool(int count, PropertyEditableVMPool _propertyVMPool)
		{
			propertyVMPool = _propertyVMPool;
			items = new List<ItemEditableViewModel>();
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				items.Add(new ItemEditableViewModel(propertyVMPool));
				FreeItems++;
			}
		}
	}
}
