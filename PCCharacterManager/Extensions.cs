using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager
{
    static class Extensions
    {

		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> itemsToAdd)
		{
			foreach (var item in itemsToAdd)
			{
				collection.Add(item);
			}
		}

		public static void Remove<T>(this ObservableCollection<T> collection, T item)
		{
			collection.Remove(item);
		}

		public static bool Contains<T>(this T[] collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}

		public static T GetRandom<T>(this T[] array)
		{
			int randomIndex = new Random().Next(0, array.Length);

			T random = array[randomIndex];

			return random;
		}
	}
}
