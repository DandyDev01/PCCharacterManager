using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public interface IPool<T>
	{
		/// <summary>
		/// gets an item from the pool
		/// </summary>
		/// <returns>item from the pool</returns>
		public T GetItem();

		/// <summary>
		/// adds a specified number of items to the pool
		/// </summary>
		/// <param name="count">number of items to add</param>
		public void Add(int count);

		/// <summary>
		/// adds an item back into the pool
		/// </summary>
		/// <param name="item">item to return to pool</param>
		public void Return(T item);	
	}
}
