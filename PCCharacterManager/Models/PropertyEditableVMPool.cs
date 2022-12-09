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
			items = new List<PropertyEditableViewModel>();
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				items.Add(new PropertyEditableViewModel());
				FreeItems++;
			}
		}
	}
}
