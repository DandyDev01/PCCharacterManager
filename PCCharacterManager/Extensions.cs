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

		/// <summary>
		/// Capitalizes the first character in the string.
		/// </summary>
		/// <param name="str">string to capitilise the first character of.</param>
		/// <returns>new string where the first letter is in upper case.</returns>
		public static string CapitalizeFirst(this string str)
		{
			if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
				return str;	

			string title = string.Empty;

			str = title.Trim();
			str = str.ToLower();
			title = str.Substring(0, 1);
			title = title.ToUpper();
			title += str.Substring(1);

			return title;
		}

		public static string CapitalizeFirst(this Enum e)
		{
			return e.ToString().CapitalizeFirst();
		}
	}
}
