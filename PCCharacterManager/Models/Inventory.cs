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
		// NOTE: add stuff so there can be an item in the inventory call capacity with properties for keeping track
		//		 of the amount of things that can be carried. this item should be hidden and can be toggled active
		//		 inactive and toggle its visibility too.

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

		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		/// <param name="item">The item to add.</param>
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


		/// <summary>
		/// Removes an item from inventory.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		public void Remove(Item item)
		{
			char firstLetter = Char.ToLower(item.Name.First());
			Items[firstLetter].Remove(item);
		}
	} // end class
}
