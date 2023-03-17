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
		/// <summary>
		/// foreach input string, create a group and add it to a table
		/// </summary>
		/// <param name="inputString">string array to group</param>
		/// <param name="breakPoints">characters that define where breaks should be</param>
		/// <returns>List of string arrays</returns>
		public static List<string[]> CreateTableGroup(string[] inputString, params char[] breakPoints)
		{
			List<string[]> table = new List<string[]>();

			foreach (var item in inputString)
			{
				string[] group = CreateGroup(item, breakPoints);
				table.Add(group);
			}

			return table;

		}

		/// <summary>
		/// break a string into a collection of strings at break points
		/// </summary>
		/// <param name="input">string to break apart</param>
		/// <param name="breakPoints">char that defines where to break at</param>
		/// <returns></returns>
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
		/// <param name="input">a string with 'x11111' at the end. Arrow x2 for example</param>
		/// <returns></returns>
		public static string RemoveQuantity(string input)
		{
			char[] characters = input.ToLower().ToArray();
			int[] index = FindAllOcurancesOfChar(input, 'x');

			for (int i = 0; i < index.Length; i++)
			{
				if (char.IsNumber(characters[index[i] + 1]) || characters[index[i] + 1] == '-')
				{
					int indexStart = index[i] + 1;
					return input.Substring(0, indexStart - 1).Trim();
				}
			}

			return input.Trim();
		}

		/// <summary>
		/// finds a numerical character at the end of a string
		/// </summary>
		/// <param name="input">string that ends with x1111. Arrowx2</param>
		/// <returns>the numerical value at the end of the string</returns>
		public static int FindQuantity(string input)
		{
			char[] characters = input.ToLower().ToArray();
			int[] indices = FindAllOcurancesOfChar(input, 'x');
			StringBuilder number = new StringBuilder();

			for (int i = 0; i < indices.Length; i++)
			{
				if (char.IsNumber(characters[indices[i] + 1]))
				{
					int indexStart = indices[i] + 1;
					int checkIndex = indexStart;
					while (char.IsNumber(characters[checkIndex]))
					{
						number.Append(characters[checkIndex]);
						if (++checkIndex > characters.Length - 1) break;
					}

					return Int32.Parse(number.ToString());
				}
				else if (characters[indices[i] + 1] == '-')
				{
					if (char.IsNumber(characters[indices[i] + 2]))
					{
						number.Append(characters[indices[i] + 1]);
					}

					int indexStart = indices[i] + 2;
					int checkIndex = indexStart;
					while (char.IsNumber(characters[checkIndex]))
					{
						number.Append(characters[checkIndex]);
						if (++checkIndex > characters.Length - 1) break;
					}

					return Int32.Parse(number.ToString());
				}
			}

			return 1;
		}

		public static int[] FindAllOcurancesOfChar(string input, char lookingFor)
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

			return index.ToArray();

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

	public static class ReadWriteJsonFile<T>
	{
		/// <summary>
		/// returns deserialized json object, null if can't
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static T? ReadFile(string filePath)
		{
			var serializedObject = File.ReadAllText(filePath);
			var objectType = JsonConvert.DeserializeObject<T>(serializedObject);

			return objectType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath">where to store the file</param>
		/// <param name="classType"></param>
		public static void WriteFile(string filePath, T classType)
		{
			var serializedObject = JsonConvert.SerializeObject(classType);
			File.WriteAllText(filePath, serializedObject);
		}
	}
}
