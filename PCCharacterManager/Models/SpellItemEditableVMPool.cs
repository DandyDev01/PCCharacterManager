using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class SpellItemEditableVMPool : IPool<SpellItemEditableViewModel>
	{
		private List<SpellItemEditableViewModel> items;

		public int AllocatedItems { get; private set; }	
		public int FreeItems { get; private set; }
		public int Count => items.Count;

		public SpellItemEditableVMPool(int count)
		{
			items = new List<SpellItemEditableViewModel>();

			Add(count);
		}

		public void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				items.Add(new SpellItemEditableViewModel());
				FreeItems++;
			}
		}

		public SpellItemEditableViewModel GetItem()
		{
			if(items.Count <= 0)
			{
				Add(1);
			}

			AllocatedItems++;
			FreeItems--;
			SpellItemEditableViewModel item = items.First();
			items.Remove(item);
			return item;
		}

		public void Return(SpellItemEditableViewModel item)
		{
			items.Add(item);
			FreeItems++;
			AllocatedItems--;
		}
	}
}
