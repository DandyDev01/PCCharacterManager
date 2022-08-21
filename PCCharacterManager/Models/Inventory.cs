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
		//[Obsolete("Use Items")]
		//public ObservableCollection<Item> All { get; private set; }
	    public Dictionary<char, ObservableCollection<Item>> Items { get; private set; }

		public Inventory()
		{
			//All = new ObservableCollection<Item>();
			Items = new Dictionary<char, ObservableCollection<Item>>();
		}

		public Inventory(IEnumerable<Item> items)
		{
			//All = new ObservableCollection<Item>();
			Items = new Dictionary<char, ObservableCollection<Item>>();
		}

		public void Add(Item item)
		{
			char firstLetter = Char.ToLower(item.Name.First());

			if (Items.ContainsKey(firstLetter))
			{
				Items[firstLetter].Add(item);
			}
			else
				Items.Add(firstLetter, new ObservableCollection<Item> { item });

		}

		public void Remove(Item item)
		{
			char firstLetter = Char.ToLower(item.Name.First());
			Items[firstLetter].Remove(item);
		}
	} // end class
}
