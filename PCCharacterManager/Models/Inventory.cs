using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Inventory
	{
		public ObservableCollection<Item> All { get; private set; }

		public Inventory()
		{
			All = new ObservableCollection<Item>();
		}

		public Inventory(IEnumerable<Item> items)
		{
			All = new ObservableCollection<Item>(items);
		}

		public void Add(Item item)
		{
			All.Add(item);
			//SortByTag(this);
		}

		public void Remove(Item item)
		{
			All.Remove(item);
			//SortByTag(this);
		}

		public static void SortByTag(Inventory inventory)
		{

		}

	} // end class
}
