using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StringFormater
	{
		// break a string into a group of strings
		public static List<string[]> CreateTableGroup(string[] inputString, params char[] breakPoints)
		{
			List<string[]> table = new List<string[]>();

			foreach (var item in inputString)
			{
				string[] group = item.Split(breakPoints);
				for (int i = 0; i < group.Length; i++)
				{
					group[i] = group[i].Trim();
				}
				table.Add(group);
			}

			return table;

		}

		public static string[] CreateGroup(string input, params char[] breakPoints)
		{
			string[] group = input.Split(breakPoints);
			for (int i = 0; i < group.Length; i++)
			{
				group[i] = group[i].Trim();
			}
			return group;
		}

		/// <summary>
		/// can only find positive numbers, format of quantity x1
		/// </summary>
		/// <param name="input">a string with 'x11111' at the end</param>
		/// <returns></returns>
		public static string RemoveQuantity(string input)
		{
			char[] characters = input.ToLower().ToArray();
			List<int> index = FindAllOcurancesOfChar(input, 'x');

			for (int i = 0; i < index.Count; i++)
			{
				if (char.IsNumber(characters[index[i] + 1]))
				{
					int indexStart = index[i] + 1;
					return input.Substring(0, indexStart - 1).Trim();

				}
			}
			return input.Trim();
		}

		// format x1
		// can only find positive numbers
		public static int FindQuantity(string input)
		{
			char[] characters = input.ToLower().ToArray();
			List<int> index = FindAllOcurancesOfChar(input, 'x');
			List<char> number = new List<char>();

			for (int i = 0; i < index.Count; i++)
			{
				if (char.IsNumber(characters[index[i] + 1]))
				{
					int indexStart = index[i] + 1;
					int checkIndex = indexStart;
					while (char.IsNumber(characters[checkIndex]))
					{

						number.Add(characters[checkIndex]);
						if (++checkIndex > characters.Length - 1)
						{
							break;
						}
					}
					// found all numeric char's
					var s = new string(number.ToArray());
					return Int32.Parse(s);
				}
			}
			return 1;
		}

		public static List<int> FindAllOcurancesOfChar(string input, char lookingFor)
		{
			char[] characters = input.ToLower().ToArray();
			List<int> index = new List<int>();

			// find all occurances of lookingFor
			for (int i = 0; i < characters.Length; i++)
			{
				if (characters[i].Equals(lookingFor))
				{
					index.Add(i);
				}
			}

			return index;

		}

		public static string FindCharSequence(char[] sequence, string search)
		{
			throw new NotImplementedException();
		}

		public static string Get1stWord(string value)
		{
			int space = value.IndexOf(' ');
			return value.Substring(0, space).Trim();
		}


	} // end StringFormater

	public static class ReadWriteJsonCollection<T>
	{
		public static List<T> ReadCollection(string filePath)
		{

			var serializedCollection = File.ReadAllText(filePath);
			var collection = JsonConvert.DeserializeObject<IEnumerable<T>>(serializedCollection);

			return new List<T>(collection);

		}

		public static void WriteCollection(string filePath, IEnumerable<T> collection)
		{
			var serializedCollection = JsonConvert.SerializeObject(collection);
			File.WriteAllText(filePath, serializedCollection);
		}

	} // end ReadJsonCollection
}
