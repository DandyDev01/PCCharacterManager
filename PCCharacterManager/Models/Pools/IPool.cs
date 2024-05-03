using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public abstract class IPool<T>
	{
		protected List<T> _items = new();

		public int AllocatedItems { get; protected set; }
		public int FreeItems { get; protected set; }
		public int Count => _items.Count;

		/// <summary>
		/// gets an item from the pool
		/// </summary>
		/// <returns>item from the pool</returns>
		public T GetItem() 
		{
			if (_items.Count <= 0)
			{
				Add(1);
			}

			AllocatedItems++;
			FreeItems--;
			T item = _items.First();
			_items.Remove(item);
			return item;
		}

		/// <summary>
		/// adds a specified number of items to the pool
		/// </summary>
		/// <param name="count">number of items to add</param>
		public abstract void Add(int count);

		/// <summary>
		/// adds an item back into the pool
		/// </summary>
		/// <param name="item">item to return to pool</param>
		public void Return(T item)
		{
			_items.Add(item);
			FreeItems++;
			AllocatedItems--;
		}	
	}
}
