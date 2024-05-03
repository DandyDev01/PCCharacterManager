using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class PropertyEditableVMPool : IPool<PropertyEditableViewModel>
	{
		public PropertyEditableVMPool(int count)
		{
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_items.Add(new PropertyEditableViewModel());
				FreeItems++;
			}
		}
	}
}
